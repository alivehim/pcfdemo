using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class NoodlemagazineDataDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IStreamAnalyzer
    {
        private string XPATH = @"//*[@class='item']";
        public string ShortIcon => "website.png";
        private readonly IEventAggregator eventAggregator;
        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IHttpService httpService;

        public NoodlemagazineDataDescription(IEventAggregator eventAggregator, IMediaSymbolDBService mediaSymbolDBService, IHttpService httpService)
        {
            this.eventAggregator = eventAggregator;
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.httpService = httpService;
        }

        public override string GetNextAddress(string address, out int page)
        {
            var localPage = 0;
            var result = Regex.Replace(address, @"movies/(?<key>[\d]*)/latest/", (mc) =>
            {
                localPage = int.Parse(mc.Groups["key"].Value);
                return $"movies/{localPage + 1}/latest/";
            });

            page = localPage + 1;
            return result;


        }

        public override string GetProceedingAddress(string address, out int page)
        {
            var localpage = 0;
            var result = Regex.Replace(address, @"movies/(?<key>[\d]*)/latest/", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"movies/{localpage - 1}/latest/";
            });

            page = localpage - 1;
            return result;

        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Noodlemagazine.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                var path = GetStoragePath();
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";
                    var namenode = categoryNode.SelectSingleNode(@".//a/div[2]/div");
                    var nameunhandle = $"{namenode.InnerText}";
                    var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//a/div/img");
                    var imageurl = $"{imageNode?.GetAttributeValue("data-src", string.Empty)}";

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Order = index,
                        Url = url,
                        StoragePath =$"{path}\\{name}.mp4",
                        MediaType = MediaSymbolType.Noodlemagazine
                    });
                    index++;
                }
            }

            return Result(data);
        }

        //<meta property="og:video" content="https://nmcorp.video/player/-101820529_456239061?m=89f27fbbb596b8fe6e03e4524a8e1945" />
        //<script type="55b718b7eb3a448fecda10c5-text/javascript">window.playlistUrl="/playlist/-101820529_456239061?h=jY4r_-iLZqNemNMUdmoFrQ&e=1712069973&f=1";window.is_mobile=false;</script>
        /*{
    "image": "https://sun9-44.userapi.com/c850736/v850736799/1f9b3b/5Hq7T5vzAQ8.jpg",
    "sources": [
        {
            "file": "https://vkvd482.mycdn.me/?expires=1712329231038&srcIp=188.68.222.232&pr=40&srcAg=CHROME&ms=45.136.22.151&type=3&sig=ZiVI3PUj3ZU&ct=0&urls=185.226.52.186&clientType=13&appId=512000384397&zs=12&id=759001254506",
            "label": "720",
            "type": "mp4"
        },
        {
            "file": "https://vkvd482.mycdn.me/?expires=1712329231038&srcIp=188.68.222.232&pr=40&srcAg=CHROME&ms=45.136.22.151&type=2&sig=hQesicJYzxQ&ct=0&urls=185.226.52.186&clientType=13&appId=512000384397&zs=12&id=759001254506",
            "label": "480",
            "type": "mp4",
            "default": true
        },
        {
            "file": "https://vkvd482.mycdn.me/?expires=1712329231038&srcIp=188.68.222.232&pr=40&srcAg=CHROME&ms=45.136.22.151&type=1&sig=4O7BgnJ24Gk&ct=0&urls=185.226.52.186&clientType=13&appId=512000384397&zs=12&id=759001254506",
            "label": "360",
            "type": "mp4"
        },
        {
            "file": "https://vkvd482.mycdn.me/?expires=1712329231038&srcIp=188.68.222.232&pr=40&srcAg=CHROME&ms=45.136.22.151&type=0&sig=USMTQ7RPvNI&ct=0&urls=185.226.52.186&clientType=13&appId=512000384397&zs=12&id=759001254506",
            "label": "240",
            "type": "mp4"
        }
    ],
    "trusted": true,
    "cdn": 0
}
         */

        private class StreamSource
        {
            public string file { get; set; }
            public string label { get; set; }
            public string type { get; set; }
        }

        private class StreamList
        {
            public IList<StreamSource> sources { get; set; }
        }

        public async Task<string> GetStreamFile(IStreamUXItemDescription item)
        {
            try
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Noodlemagazine.ToString());
                var source = await HttpHelper.GetUrlContentAsync(item.Url, Encoding.GetEncoding("utf-8"));

                if (!string.IsNullOrEmpty(source))
                {

                    var mc = Regex.Match(source, @"<meta property=""og:video"" content=""(?<key>.*?)"" />");
                    if (mc.Success)
                    {
                        item.StreamUri = mc.Groups["key"].Value;

                        item.Info($"get the content url {item.StreamUri}");
                        item.TaskStage = TaskStage.Prepare;
                        return item.StreamUri;
                        var htmlRes = await httpService.GetPageSourceAsync(item.StreamUri);
                        var playerHtml = htmlRes.Item1;

                        if (!string.IsNullOrEmpty(playerHtml))
                        {
                            var downloadmc = Regex.Match(playerHtml, @"<script type=""55b718b7eb3a448fecda10c5-text/javascript"">window.playlistUrl=""(?<key>.*?)"";window.is_mobile=false;</script>");
                            if (downloadmc.Success)
                            {
                                item.StreamUri = $"{symbol.Address}{downloadmc.Groups["key"].Value}";

                                var downloadHtml = await HttpHelper.GetUrlContentAsync(item.StreamUri, Encoding.GetEncoding("utf-8"));

                                if (!string.IsNullOrEmpty(downloadHtml))
                                {
                                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<StreamList>(downloadHtml);

                                    item.StreamUri = obj.sources.SingleOrDefault(p => p.label == "720")?.file;
                                    item.Info("get mp4 file successfully");
                                    //eventAggregator.GetEvent<MediaStreamEvent>().Publish(new StreamMessage { MesasgeType = MesasgeType.StreamFileCompleted, Id = item.ID });

                                    return item.StreamUri;
                                }
                            }
                        }


                    }
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                item.TaskStage = TaskStage.Error;
                item.Error(ex.ToString());
                return string.Empty;
            }
        }
    }
}

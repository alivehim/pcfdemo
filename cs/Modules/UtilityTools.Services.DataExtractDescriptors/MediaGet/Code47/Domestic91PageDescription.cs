using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
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
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class Domestic91PageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IStreamAnalyzer
    {
        private string XPATH = @"//*[@class='video-elem']";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public Domestic91PageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.eventAggregator = eventAggregator;
        }

        public override string GetNextAddress(string address, out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetProceedingAddressWithEqualSign(address, out page);
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {

                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";
                    var previewurl = innerNode.GetAttributeValue("data-previewvideo", string.Empty);
                    var durationnode = categoryNode.SelectSingleNode(@".//a/small");
                    var duration = durationnode?.InnerText;
                    var namenode = categoryNode.SelectSingleNode(@".//a[2]");
                    var name = HttpUtility.HtmlDecode(ContentReplace(namenode.InnerText).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//a/div[2]");
                    var style = imageNode.GetAttributeValue("style", string.Empty);
                    var imageurl = string.Empty;
                    var mc = Regex.Match(style, @"'(?<key>.*?)'");
                    if (mc.Success)
                    {
                        imageurl = mc.Groups["key"].Value.ToString();
                    }

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = $"https:{imageurl}",
                        Url = $"{symbol.Address}{url}",
                        Duration = duration,
                        StoragePath = $"{GetStoragePath()}\\{name}.mp4",
                        MediaType = MediaSymbolType.DomesticNineOne
                    });
                    index++;
                }
            }

            return Result(data);
        }

        public async Task<string> GetStreamFile(IStreamUXItemDescription item)
        {

            try
            {
                var source = await HttpHelper.GetUrlContentAsync(item.Url, Encoding.UTF8);
                if (!string.IsNullOrEmpty(source))
                {

                    var mc = Regex.Match(source, @"data-src=""(?<key>.*?)""");
                    if (mc.Success)
                    {
                        item.StreamUri = HttpUtility.HtmlDecode(mc.Groups["key"].Value.ToString());

                        item.TaskStage = TaskStage.Prepared;

                        eventAggregator.GetEvent<MediaStreamEvent>().Publish(new StreamMessage { MesasgeType = MesasgeType.StreamFileCompleted, Id = item.ID });
                        return item.StreamUri;
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

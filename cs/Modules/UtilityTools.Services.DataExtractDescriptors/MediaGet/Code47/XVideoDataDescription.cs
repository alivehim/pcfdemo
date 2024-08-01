using CefSharp.DevTools.Overlay;
using CefSharp.DevTools.PerformanceTimeline;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Extensions;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class XVideoDataDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IStreamAnalyzer
    {
        private string XPATH = @"//*[@class='mozaique cust-nb-cols']/div";
        //private string XPATH2 = @"//*[@class='mozaique cust-nb-cols post-blocks']/div";
        public string ShortIcon => "website.png";
        private readonly IEventAggregator eventAggregator;
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public XVideoDataDescription(IEventAggregator eventAggregator, IMediaSymbolDBService mediaSymbolDBService)
        {
            this.eventAggregator = eventAggregator;
            this.mediaSymbolDBService = mediaSymbolDBService;
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
            string html = await HttpHelper.GetUrlContentAsync2(MediaGetContext.Key);
            if (!string.IsNullOrEmpty(html))
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.XVideo.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                var path = GetStoragePath();
                if (categoryNodeList != null)
                {
                    int index = 0;

                    foreach (HtmlNode categoryNode in categoryNodeList)
                    {

                        var innerNode = categoryNode.SelectSingleNode(@".//div/div/a");
                        if (innerNode != null)
                        {
                            var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";

                            var nameunhandle = categoryNode.SelectSingleNode(@".//div[2]/p/a");
                            var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle.InnerText).Replace(".", ""));
                            var imageNode = categoryNode.SelectSingleNode(@".//div/div/a/img");
                            var imageurl = $"{imageNode?.GetAttributeValue("data-src", string.Empty)}";

                            data.Add(new MediaDataDescription
                            {
                                Name = name,
                                ImageUrl = imageurl,
                                Order = index,
                                Url = url,
                                StoragePath = $"{path}\\{name}.mp4",
                                MediaType = MediaSymbolType.XVideo
                            });
                            index++;
                        }

                    }
                }
                else
                {
                    int index = 0;
                    var res = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponesVideos>(html);

                    foreach (var item in res.videos)
                    {
                        data.Add(new MediaDataDescription
                        {
                            Name = HttpUtility.HtmlDecode(ContentReplace(item.t ?? item.title).Replace(".", "")),
                            ImageUrl = item.i??item.thumb_url,
                            Order = index,
                            Url = $"{symbol.Address}{item.u??item.url}",
                            StoragePath = $"{path}\\{item.t??item.title}.mp4",
                            MediaType = MediaSymbolType.XVideo
                        });
                        index++;
                    }


                }
            }


            return Result(data);
        }

        private class ResponseVideo
        {
            public string id { get; set; }
            public string i { get; set; }
            public string thumb_url { get; set; }
            public string t { get; set; }
            public string title { get; set; }
            public string u { get; set; }
            public string url { get; set; }
        }
        private class ResponesVideos
        {
            public IList<ResponseVideo> videos { get; set; }
        }

        public async Task<string> GetStreamFile(IStreamUXItemDescription item)
        {
            try
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.XVideo.ToString());
                var source = await HttpHelper.GetUrlContentAsync2(item.Url);

                if (!string.IsNullOrEmpty(source))
                {

                    var mc = Regex.Match(source, @"html5player.setVideoHLS\('(?<key>.*?)'\);");
                    if (mc.Success)
                    {
                        item.StreamUri = mc.Groups["key"].Value.ToString();
                        var hlsHtml = await HttpHelper.GetUrlContentAsync2(item.StreamUri);
                        if (!string.IsNullOrEmpty(hlsHtml))
                        {

                            var labels = new string[] { "1080", "720", "480", "360" };

                            var lines = hlsHtml.Split('\n');
                            foreach (var label in labels)
                            {
                                if (hlsHtml.Contains($"{label}p"))
                                {
                                    var line = lines.First(p => p.Contains($"{label}p"));
                                    var index = lines.IndexOf(line);
                                    item.Info("get mp4 file successfully");
                                    item.StreamUri = item.StreamUri.Replace("hls.m3u8", lines[index + 1]);
                                    item.TaskStage = TaskStage.Prepared;
                                    eventAggregator.GetEvent<MediaStreamEvent>().Publish(new StreamMessage { MesasgeType = MesasgeType.StreamFileCompleted, Id = item.ID });
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

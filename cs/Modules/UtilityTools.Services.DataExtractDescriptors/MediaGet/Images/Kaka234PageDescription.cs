using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class Kaka234PageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IImageCollector
    {
        private string XPATH = @"//*[@class='c s_li zxgx_list l']/ul/li";
        private string IMAGEXPATH = @"//*[@class='content']/img";
        public string ShortIcon => "website.png";
        private readonly IEventAggregator eventAggregator;
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public Kaka234PageDescription(IEventAggregator eventAggregator, IMediaSymbolDBService mediaSymbolDBService)
        {
            this.eventAggregator = eventAggregator;
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address, out int page)
        {

            int localpage = 0;
            var result = Regex.Replace(address, @"list_(?<key>[\d]*).html", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"list_{localpage + 1}.html";
            });

            page = localpage + 1;
            return result;

        }

        public override string GetProceedingAddress(string address, out int page)
        {
            int localpage = 0;
            var result = Regex.Replace(address, @"list_(?<key>[\d]*).html", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"list_{localpage - 1}.html";
            });
            page = localpage - 1; return result;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("gb2312"));
            if (!string.IsNullOrEmpty(html))
            {

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                    var name = HttpUtility.HtmlDecode(ContentReplace(innerNode.GetAttributeValue("title", string.Empty)).Replace(".", ""));

                    var imageNode = categoryNode.SelectSingleNode(@".//a/img");
                    var imageurl = $"{imageNode?.GetAttributeValue("src", string.Empty)}";

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Order = index,
                        Url = url,
                        MediaType = MediaSymbolType.Kaka234,
                        StoragePath = $"{Settings.Current.MangaFolder}\\Kaka234\\{name}"
                    });
                    index++;
                }
            }

            return Result(data);
        }

        private int GetPagesCount(string html)
        {
            var match = Regex.Match(html, @"共(?<key>.*?)页:");
            if (match.Success)
            {
                var value = match.Groups["key"].Value;

                return int.Parse(value);

            }
            return 0;
        }

        private IList<string> GetPages(string address, int pages)
        {
            var result = new List<string>();

            result.Add(address);
            int index = 2;
            do
            {
                var newAddress = address.Replace(".html", $"_{index}.html");
                index++;
                result.Add(newAddress);

            } while (index <= pages);
            return result;

        }

        public async Task<IList<MediaDataDescription>> GetImagesAsync(string pipAddress)
        {
            string html = await HttpHelper.GetUrlContentAsync(pipAddress, Encoding.GetEncoding("gb2312"));
            var result = new List<MediaDataDescription>();

            if (!string.IsNullOrEmpty(html))
            {

                var num = GetPagesCount(html);

                var pages = GetPages(pipAddress, num);
                int index = 0;

                foreach (var page in pages)
                {
                    string content = await HttpHelper.GetUrlContentAsync(page, Encoding.GetEncoding("gb2312"));

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(content);
                    HtmlNode rootNode = document.DocumentNode;
                    HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(IMAGEXPATH);


                    foreach (var imageNode in categoryNodeList)
                    {
                        var url = imageNode.GetAttributeValue("src", string.Empty);

                        result.Add(new MediaDataDescription
                        {
                            ImageUrl = url,
                            Order = index,
                            Url = url,
                        });
                        index++;

                    }
                }


                //var numreg = Regex.Match(html, @"展开全图\(1/(?<key>[\d]*)\)");
                //if (numreg.Success)
                //{
                //    var num = int.Parse(numreg.Groups["key"].Value);

                //    var urlreg = Regex.Match(html, @"<div class=""content"" id=""content""><img src=""(?<key>.*?)""");
                //    if (urlreg.Success)
                //    {
                //        var address = urlreg.Groups["key"].Value;

                //        var domainreg = Regex.Match(address, @"(?<key>.*?)/1.jpg");
                //        if (domainreg.Success)
                //        {
                //            var domain = domainreg.Groups["key"].Value;
                //            int index = 0;

                //            for (; index <= num; index++)
                //            {
                //                result.Add(new MediaDataDescription
                //                {
                //                    ImageUrl = $@"{domain}/{index + 1}.jpg",
                //                    Order = index,
                //                    Url = $@"{domain}/{index + 1}.jpg",
                //                });
                //            }
                //        }
                //    }
                //}


            }
            return result;
        }
    }
}

using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class SeehimfuckPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='item-video hover']";
        public string ShortIcon => "website.png";
        private readonly IEventAggregator eventAggregator;
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public SeehimfuckPageDescription(IEventAggregator eventAggregator, IMediaSymbolDBService mediaSymbolDBService)
        {
            this.eventAggregator = eventAggregator;
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address, out int page)
        {
            var localPage = 0;
            var result= Regex.Replace(address, @"movies/(?<key>[\d]*)/latest/", (mc) =>
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
            var result= Regex.Replace(address, @"movies/(?<key>[\d]*)/latest/", (mc) =>
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
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Hussiepass.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//div/a");
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                    var nameunhandle = $"{innerNode.GetAttributeValue("title", string.Empty)}";
                    var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//div/a/img");
                    var imageurl = $"{symbol.Address}{imageNode?.GetAttributeValue("src0_1x", string.Empty)}";

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Order = index,
                        Url = url,
                        MediaType = MediaSymbolType.Hussiepass
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

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
    public class Tiny4kPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='video-thumbnail flex flex-col space-y-1']";
        public string ShortIcon => "website.png";
        private readonly IEventAggregator eventAggregator;
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public Tiny4kPageDescription(IEventAggregator eventAggregator, IMediaSymbolDBService mediaSymbolDBService)
        {
            this.eventAggregator = eventAggregator;
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address,out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Tiny4K.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//div/div/a");
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                    var namenode = innerNode.SelectSingleNode(@".//div[2]/div/img");
                    //var nameunhandle = $"{innerNode.GetAttributeValue("title", string.Empty)}";
                    var name = namenode.GetAttributeValue("alt",string.Empty);
                    //var imageNode = categoryNode.SelectSingleNode(@".//div/a/img");
                    var imageurl = $"{symbol.Address}{namenode?.GetAttributeValue("src", string.Empty)}";

                    var actressnode = innerNode.SelectSingleNode(@".//div[2]/div[2]");

                    var videonode = innerNode.SelectSingleNode(@".//video");

                    imageurl = $"{videonode?.GetAttributeValue("poster", string.Empty)}";
                    var name2 = actressnode.InnerText;
                    data.Add(new MediaDataDescription
                    {
                        Name = $"{name} {name2}",
                        ImageUrl = imageurl,
                        Order = index,
                        Url = url,
                        MediaType = MediaSymbolType.Tiny4K
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

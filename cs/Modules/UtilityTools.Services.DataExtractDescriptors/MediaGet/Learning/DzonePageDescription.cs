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
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;
using System.Net.WebSockets;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class DzonePageDescription : BaseMediaPageDescription, IGrabArtcleContent,IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='article-block']/div";
        private string CONTENTXPATH = @"//*[@class='content-html']";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public DzonePageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.eventAggregator = eventAggregator;
        }

        public override string GetNextAddress(string address,out int page)
        {
            //https://carldesouza.com/posts/page/2/
            int localpage = 0;

            var nextaddress = Regex.Replace(address, @"/page/(?<key>[\d]*)/", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"/page/{localpage + 1}/";
            });

            if (address == nextaddress)
            {
                nextaddress = $"{address}/page/2/";
            }

            page = localpage + 1;
            return nextaddress;
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            int localpage = 0;
            var result =  Regex.Replace(address, @"/page/(?<key>[\d]*)/", (mc) =>
            {
                var val = int.Parse(mc.Groups["key"].Value);
                return $"/page/{val - 1}/";
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

                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Dzone.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//div[2]/a");
                    var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";
                    var name = HttpUtility.HtmlDecode(ContentReplace(innerNode.InnerText).Replace(".", ""));

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        Url = url,
                        MediaType = MediaSymbolType.Dzone,
                        Order=index
                    });
                    index++;
                }
            }

            return Result(data);
        }

        public async Task<string> GetContentAsync(string address)
        {
            string html = await HttpHelper.GetUrlContentAsync(address, Encoding.GetEncoding("utf-8"));
            if (string.IsNullOrEmpty(html))
            {
                throw new Exception("fetch content empty");
            }

            //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNode rootNode = document.DocumentNode;
            var contentNode = rootNode.SelectSingleNode(CONTENTXPATH);

            var content = contentNode.InnerHtml;

            return content;
        }
    }
}

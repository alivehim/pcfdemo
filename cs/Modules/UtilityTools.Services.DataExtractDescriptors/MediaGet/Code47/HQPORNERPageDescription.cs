using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Data;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class HQPORNERPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='6u']/section";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public HQPORNERPageDescription(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address, out int page)
        {
            return GetNextAddressWithPage(address, out page);
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetProceedingAddressWithPage(address, out page);
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {

               var symbol= mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.HQPorner.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";

                    var nameunhandle = categoryNode.SelectSingleNode(@".//div[2]/h3/a");
                    var name = HttpUtility.HtmlDecode(nameunhandle.InnerText);
                    var imageNode = categoryNode.SelectSingleNode(@".//a/div/div[1]");
                    var imagestring = imageNode?.GetAttributeValue("onmouseover", string.Empty);

                    var mc=Regex.Match(imagestring, @"changeImage\(\""\/\/(?<key>.*?)\""");
                    var imageurl = $"https://{ mc.Groups["key"].Value.ToString()}";
                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Order = index,
                        Url = url,
                        MediaType = MediaSymbolType.HQPorner
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

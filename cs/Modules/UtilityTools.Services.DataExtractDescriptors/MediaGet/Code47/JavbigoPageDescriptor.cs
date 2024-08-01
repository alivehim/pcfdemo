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
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class JavbigoPageDescriptor : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='videos-list']/article";
        public string ShortIcon => "website.png";

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
            string html =await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
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

                    var nameunhandle = $"{innerNode.GetAttributeValue("title", string.Empty)}";
                    var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//a/div/div/img");
                    var imageurl = imageNode?.GetAttributeValue("data-src", string.Empty);

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Order = index,
                        Url = url,
                        MediaType = MediaSymbolType.Javbigo
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

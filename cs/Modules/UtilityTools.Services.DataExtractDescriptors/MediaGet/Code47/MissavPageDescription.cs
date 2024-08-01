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
    public class MissavPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='grid grid-cols-2 md:grid-cols-3 xl:grid-cols-4 gap-5']/div";
        public string ShortIcon => "website.png";

        public override string GetNextAddress(string address, out int page)
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

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//div/div[2]/a");
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                    var name = HttpUtility.HtmlDecode(ContentReplace(innerNode.InnerText).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//div/div/a/img");
                    var imageurl = imageNode.GetAttributeValue("data-src", string.Empty);

                    var videoNode = categoryNode.SelectSingleNode(@".//div/div/a/video");
                    var previewUrl = videoNode.GetAttributeValue("data-src", string.Empty);

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Url = url,
                        Order = index,
                        MediaType = MediaSymbolType.Missav,
                        PreviewUrl= previewUrl
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

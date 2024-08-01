using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class PPVPageDescriptor : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private readonly string XPATH = "//*[@class='videos-list']/article";

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
            string html = await GetHtmlSource(MediaGetContext.Key);
            if (!string.IsNullOrEmpty(html))
            {

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                //sub images
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {
                    try
                    {
                        var hrefnode = categoryNode.SelectSingleNode(".//a");
                        var url = $"{hrefnode.GetAttributeValue("href", string.Empty)}";

                        var name = hrefnode.GetAttributeValue("title", string.Empty);

                        var imagenode = categoryNode.SelectSingleNode(".//a//div//img");

                        var imageurl = imagenode.GetAttributeValue("data-lazy-src", string.Empty);

                        if (string.IsNullOrEmpty(imageurl))
                        {
                            imageurl = imagenode.GetAttributeValue("data-src", string.Empty);
                        }
                        var item = new MediaDataDescription
                        {
                            Url = url,
                            Name = name,
                            ImageUrl = imageurl,
                            MediaType = MediaSymbolType.PPV
                        };

                        data.Add(item);
                    }
                    catch
                    {

                    }
                    index++;
                }
            }

            return Result(data);
        }
    }
}

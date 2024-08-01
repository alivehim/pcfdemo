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
    public class FXPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private readonly string XPATH = "//*[@class='listContent stars sizeDVDLarge']/li";

        public string ShortIcon => "website.png";

        public override string GetNextAddress(string address, out int page)
        {

            var localPage = 0;

            var nextAddress = Regex.Replace(address, @"\/(?<key>[\d]*)$", (mc) =>
            {
                localPage = int.Parse(mc.Groups["key"].Value);
                return $"/{localPage + 1}";
            });

            if (nextAddress == address)
            {
                nextAddress = nextAddress + "/2";
            }

            page = localPage + 1;
            return nextAddress;
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            var localPage = 0;
            var nextAddress = Regex.Replace(address, @"\/(?<key>[\d]*)$", (mc) =>
            {
                localPage = int.Parse(mc.Groups["key"].Value);
                return $"/{localPage - 1}";
            });

            if (nextAddress == address)
            {
                nextAddress = nextAddress + "/2";
            }

            page = localPage - 1;
            return nextAddress;
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
                        var style = hrefnode.GetAttributeValue("style", string.Empty);
                        if (!string.IsNullOrEmpty(style))
                        {
                            //width:313px;height:209px;background-image:url(http://www.fxporn.net/theme/thumb/15330372315b6042d49ac90.jpg)

                            Regex reg = new Regex(@"\((?<key>.*?)\)");
                            var mc = reg.Match(style);
                            if (mc.Success)
                            {
                                var imageurl = mc.Groups["key"].Value;
                                var item = new MediaDataDescription
                                {
                                    Url = url,
                                    Name = name,
                                    ImageUrl = imageurl,
                                    Order = index,
                                    MediaType = MediaSymbolType.FX
                                };

                                data.Add(item);
                            }
                        }
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

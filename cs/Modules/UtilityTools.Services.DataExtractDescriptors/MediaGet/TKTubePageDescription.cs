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
    public class TKTubePageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='item  ']";
        public string ShortIcon => "website.png";

        public override string GetNextAddress(string address,out int page)
        {
            var localPage = 0;
            var result= Regex.Replace(address, @"from=(?<key>[\d]*)", (mc) =>
            {
                localPage = int.Parse(mc.Groups["key"].Value);
                return $"from={localPage + 1}";
            });


            page = localPage + 1;return result;

        }

        public override string GetProceedingAddress(string address, out int page)
        {
            var localPage = 0;
            var result= Regex.Replace(address, @"from=(?<key>[\d]*)", (mc) =>
            {
                localPage = int.Parse(mc.Groups["key"].Value);
                return $"from={localPage - 1}";
            });


            page = localPage - 1;

            return result;
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
                    var namenode = categoryNode.SelectSingleNode(@".//a/strong");
                    var name = HttpUtility.HtmlDecode(ContentReplace(namenode.InnerText).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//a/div/img");
                    var imageurl = imageNode.GetAttributeValue("src", string.Empty);

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Url = url,
                        MediaType = MediaSymbolType.TKTube
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

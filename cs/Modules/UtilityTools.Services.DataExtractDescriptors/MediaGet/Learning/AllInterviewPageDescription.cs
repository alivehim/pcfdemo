using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class AllInterviewPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//div[@class='panel-body']/div[@class='row text-center']";
        public string ShortIcon => "website.png";
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public AllInterviewPageDescription(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address,out int page)
        {
            //https://www.allinterview.com/interview-questions/435-3/project-planning.html
            int localpage = 0;
            var nextAddress =   Regex.Replace(address, @"/(?<key>[\d]*)-(?<index>[\d]*)/", (mc) =>
            {
                localpage = int.Parse(mc.Groups["index"].Value);
                var key = int.Parse(mc.Groups["key"].Value);
                return $"/{key}-{localpage + 1}/";

            });

            if(nextAddress == address)
            {
                nextAddress= Regex.Replace(address, @"/(?<key>(0|[0-9][0-9]*))/", (mc) =>
                {
                    var key = int.Parse(mc.Groups["key"].Value);
                    return $"/{key}-2/";

                });
                page = 2;
                return nextAddress;
            }
            else
            {
                page = localpage + 1;
                return nextAddress;
            }
          
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            int localpage = 0;
            var result= Regex.Replace(address, @"/(?<key>[\d]*)-(?<index>[\d]*)/", (mc) =>
            {
                var val = int.Parse(mc.Groups["index"].Value);
                var key = int.Parse(mc.Groups["key"].Value);
                return $"/{key}-{val - 1}/";

            });

            page = localpage;
            return result;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.AllInterview.ToString());
            if (!string.IsNullOrEmpty(html))
            {

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//span[2]/a");

                    var title = innerNode.GetAttributeValue("title", string.Empty);
                    if (title != "Post New Answer")
                    {
                        var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";

                        var nameunhandle = categoryNode.SelectSingleNode(".//span/p");
                        var name = HttpUtility.HtmlDecode(nameunhandle.InnerText);

                        data.Add(new MediaDataDescription
                        {
                            Name = name,
                            Url = url,
                            MediaType = MediaSymbolType.AllInterview,
                            Order = index,
                        });
                        index++;
                    }
                 
                }
            }

            return Result(data);
        }
    }
}

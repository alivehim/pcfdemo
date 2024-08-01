using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class QSBDCPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='e2']/li";
        public string ShortIcon => "website.png";
        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IHttpService httpService;

        public QSBDCPageDescription(IMediaSymbolDBService mediaSymbolDBService, IHttpService httpService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.httpService = httpService;
        }

        public override string GetNextAddress(string address, out int page)
        {
            int localpage = 0;
            //http://skill.qsbdc.com/dapei/chuji/list_35_2.html
            var result = Regex.Replace(address, @"list_(?<cateogry>[\d]*)_(?<page>[\d]*)", (mc) =>
            {
                localpage = int.Parse(mc.Groups["page"].Value);
                var category = int.Parse(mc.Groups["cateogry"].Value);
                return $"list_{category}_{localpage + 1}";
            });

            page = localpage + 1;
            return result;

        }

        public override string GetProceedingAddress(string address, out int page)
        {
            int localpage = 0;
            var result = Regex.Replace(address, @"list_(?<cateogry>[\d]*)_(?<page>[\d]*)", (mc) =>
         {
             localpage = int.Parse(mc.Groups["page"].Value);
             var category = int.Parse(mc.Groups["cateogry"].Value);
             return $"list_{category}_{localpage - 1}";
         });

            page = localpage - 1;
            return result;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            //string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            string html = await this.httpService.GetHtmlSourceAsync(MediaGetContext.Key);
            if (!string.IsNullOrEmpty(html))
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.QSBDC.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 1;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a[2]");
                    var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";

                    var nameunhandle = $"{innerNode.InnerText}";
                    var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle).Replace(".", ""));

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        Order = index,
                        Url = url,
                        MediaType = MediaSymbolType.QSBDC,
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

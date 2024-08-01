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
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class SoushuPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@id='threadlisttableid']/tbody";
        public string ShortIcon => "website.png";
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public SoushuPageDescription(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address, out int page)
        {
            var localpage = 0;
            var result= Regex.Replace(address, @"page=(?<key>[\d]*)", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"page={localpage + 1}";
            });


            page = localpage + 1;
            return result;
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            var localpage = 0;
            var result= Regex.Replace(address, @"page=(?<key>[\d]*)", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"page={localpage + 1}";
            });

            page = localpage - 1;return result; 
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("gbk"));
            if (!string.IsNullOrEmpty(html))
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Soushu.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList.Skip(1))
                {


                    try
                    {
                        var innerNode = categoryNode.SelectSingleNode(@".//tr/th/a[2]");
                        var url = $"{symbol.Address}/{HttpUtility.HtmlDecode(innerNode.GetAttributeValue("href", string.Empty))}";

                        var nameunhandle = $"{innerNode.InnerText}";
                        var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle).Replace(".", ""));

                        if(name != "隐藏置顶帖")
                        {
                            data.Add(new MediaDataDescription
                            {
                                Name = name,
                                Url = url,
                                MediaType = MediaSymbolType.Soushu,
                                Order = index
                            });
                            index++;
                        }
                       
                    }
                    catch
                    {

                    }

                }
            }

            return Result(data);
        }
    }
}

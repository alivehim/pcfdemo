using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Services.D365
{
    public class MicrosoftLearnService : IMicrosoftLearnService
    {

        private string XPATH = @"//*[@id='unit-list']/li";
        private string CONTENTXPATH = @"//*[@class='modular-content-container has-body-background box']";
        public async Task<List<MediaDataDescription>> GetChatpersAsync(string address)
        {
            var data = new List<MediaDataDescription>();
            var header = new Dictionary<string, string>() { { "accept-language", "en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7" } };
            string html = await HttpHelper.GetUrlContentAsync(address, Encoding.GetEncoding("utf-8"), string.Empty, false, header);
            if (!string.IsNullOrEmpty(html))
            {

                //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//div/a");
                    var url = $"{address}{innerNode.GetAttributeValue("href", string.Empty)}";
                    var name = HttpUtility.HtmlDecode(innerNode.InnerText);

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        Url = url,
                        Order = index
                    });
                    index++;

                }

            }
            return data;
        }

        public async Task<string> GetChatperContentAsync(string address)
        {
            var header = new Dictionary<string, string>() { { "accept-language", "en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7" } };
            string html = await HttpHelper.GetUrlContentAsync(address, Encoding.GetEncoding("utf-8"), string.Empty, false, header);
            if (string.IsNullOrEmpty(html))
            {
                throw new Exception("fetch content empty");
            }

            //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNode rootNode = document.DocumentNode;
            var contentNode = rootNode.SelectSingleNode(CONTENTXPATH);

            var content =  contentNode.InnerHtml;


            content= Regex.Replace(content, @"<h1 class=""margin-right-xxl-desktop"">[\w\W]*</h1>", (mc) => { return string.Empty; });
            content= Regex.Replace(content, @"<div id=""next-section""[\w\W]*</div>", (mc) => { return string.Empty; });

            return content;
           
        }
    }
}

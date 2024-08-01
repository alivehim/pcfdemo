using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.Core;

namespace UtilityTools.Services.Core
{
    public class AllInterviewService: IAllInterviewService
    {
        private string XPATH = @"//*[@class='panel panel-default']";
        public async Task<IList<string>> GetAnswersAsync(string url)
        {

            var data = new List<string>();
            string html = await HttpHelper.GetUrlContentAsync(url, Encoding.GetEncoding("gbk"));
            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);

                if (categoryNodeList != null)
                {
                    int index = 0;
                    foreach (HtmlNode categoryNode in categoryNodeList)
                    {

                            var node = categoryNode.SelectSingleNode(".//div[2]/p[2]");
                        if(node != null)
                        {
                            var content = HttpUtility.HtmlDecode(node.InnerHtml);

                            data.Add(content);
                            index++;
                        }
                      
                    }
                }
            }


            return data;
        }
    }
}

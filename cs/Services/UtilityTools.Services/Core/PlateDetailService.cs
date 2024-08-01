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

namespace UtilityTools.Services.Core
{
    public class PlateDetailService : IPlateDetailService
    {
        //private string XPATH = @"//*[@class='ask_date']";
        private string XPATH = @"//*[@class='t_f']";
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

                if(categoryNodeList!=null)
                {
                    int index = 0;
                    foreach (HtmlNode categoryNode in categoryNodeList.Skip(1))
                    {


                        var content = HttpUtility.HtmlDecode(categoryNode.InnerHtml);

                        data.Add(content);
                        index++;
                    }
                }
            }
             

            return data;
        }
    }
}

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.Core;

namespace UtilityTools.Services.Core.Magnet
{
    public class JAVDBMagnetService : IJAVDBMagnetService
    {
        private readonly string XPATH = @"//*[@id='magnets-content']/div";
        private readonly string IMAGEXPATH = @"//*[@class='tile-images preview-images']/a";

        public async Task<(IList<MagnetDescription>, IList<string>)> GetMagnetLinksAsync(string address)
        {

            string html = await HttpHelper.GetUrlContentAsync(address, Encoding.GetEncoding("utf-8"));
            if (string.IsNullOrEmpty(html))
            {
                throw new Exception("fetch content empty");
            }

            //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNode rootNode = document.DocumentNode;

            HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
            HtmlNodeCollection ImageNodeList = rootNode.SelectNodes(IMAGEXPATH);
            var list = new List<MagnetDescription>();
            var imagelist = new List<string>();
            if (categoryNodeList != null)
            {
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {

                    var hrefNode = categoryNode.SelectSingleNode(@".//div/a");
                    var url = $"{hrefNode.GetAttributeValue("href", string.Empty)}";

                    var nameNode = categoryNode.SelectSingleNode(@".//div/a/span");
                    var name = HttpUtility.HtmlDecode(nameNode.InnerText).Replace(".", "");

                    var sizeNode = categoryNode.SelectSingleNode(@".//div/a/span[2]");
                    var dateNode = categoryNode.SelectSingleNode(@".//div[2]/span");

                    list.Add(new MagnetDescription
                    {
                        FileName = name,
                        Address = url,
                        Size = sizeNode?.InnerText,
                        Date = dateNode?.InnerText
                    });
                    index++;
                }

                foreach (HtmlNode categoryNode in ImageNodeList)
                {
                    //var imgNode = categoryNode.SelectSingleNode(@".//img");

                    //imagelist.Add(imgNode.GetAttributeValue("src",string.Empty));
                    var imageurl = categoryNode.GetAttributeValue("href", string.Empty);
                    if (imageurl.EndsWith(".jpg"))
                    {
                        imagelist.Add(categoryNode.GetAttributeValue("href", string.Empty));
                    }


                }
            }

            return (list, imagelist);
        }

    }
}

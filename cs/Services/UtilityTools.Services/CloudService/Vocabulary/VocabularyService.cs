using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.Vocabulary;

namespace UtilityTools.Services.Vocabulary
{
    public class VocabularyService : IVocabularyService
    {
        private readonly string XPATH = "//*[@class='content']";
        public async Task<string> GetContent(string url)
        {
            var html = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8);

            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                var node = rootNode.SelectSingleNode(XPATH);

                return node.InnerHtml;
            }

            throw new Exception("get html content error");

        }
    }
}

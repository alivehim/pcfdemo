using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.CloudService.Vocabulary;

namespace UtilityTools.Services.CloudService.Vocabulary
{
    public class DictionaryService : IDictionaryService
    {
        private const string URL = "https://www.xxenglish.com";
        private string XPATH = @"//div[@class='container']/article";

        private async Task<string> GetContentAsync(string url)
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
        public async Task<string> GetBasicConceptAsync(string vocabulary)
        {
            var url = $@"{URL}/wd/{vocabulary}";

            return await GetContentAsync(url);
        }

        public async Task<string> GetExampleAsync(string vocabulary)
        {
            var url = $@"{URL}/w5/{vocabulary}";

            return await GetContentAsync(url);
        }

        public async Task<string> GetWordRootAsync(string vocabulary)
        {
            var url = $@"{URL}/w8/{vocabulary}";

            return await GetContentAsync(url);
        }

        public async Task<string> GetVariantsAsync(string vocabulary)
        {
            var url = $@"{URL}/w1/{vocabulary}";

            return await GetContentAsync(url);
        }

        public async Task<string> GetPhraseAsync(string vocabulary)
        {
            var url = $@"{URL}/w6/{vocabulary}";

            return await GetContentAsync(url);
        }

    }
}

using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class MicrosoftLearnPageDescsription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='card-template']";
        private string SUBXPATH = @"//*[@class='column is-auto padding-none']";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public MicrosoftLearnPageDescsription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.eventAggregator = eventAggregator;
        }

        public override string GetNextAddress(string address, out int page)
        {
            return GetNextAddressWithPage(address, out page);
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetProceedingAddressWithPage(address, out page);
        }


        public class StudyGuideResponse
        {
            public string id { get; set; }

            public int totalItems { get; set; }

            public IList<StudyGuideItem> items { get; set; }

        }

        public class StudyGuideItem
        {
            public string id { get; set; }

            public string type { get; set; }
            public StudyGuidItemData data { get; set; }
        }

        public class StudyGuidItemData
        {
            public string url { get; set; }
            public string title { get; set; }
            public string iconUrl { get; set; }

            public IList<ModuleItem> modules { get; set; }
        }


        public class ModuleResponse
        {
            public IList<ModuleItem> modules { get; set; }
        }

        public class ModuleItem
        {
            public IList<ModuleParent> parents { get; set; }
            public IList<ModuleUnit> units { get; set; }

            public ModuleAchievement achievement { get; set; }

            public string title { get; set; }

            public string iconUrl { get; set; }
        }

        public class ModuleAchievement
        {
            public string title { get; set; }
            public string uid { get; set; }
        }

        public class ModuleParent
        {
            public string title { get; set; }
            public string url { get; set; }
        }

        public class ModuleUnit
        {
            public string type { get; set; }
            public string title { get; set; }

            public string url { get; set; }
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string content = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(content))
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.MicrosoftLearn.ToString());
                var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<StudyGuideResponse>(content);


                if (jsonResult.items != null)
                {
                    foreach (var item in jsonResult.items)
                    {
                        if (item.type == "path" && item.data.modules != null)
                        {
                            //var pathurl = $"{symbol.Address}{item.data.url}";

                            //data.AddRange(RetrieveLearnPathArticle(pathurl));

                            foreach (var module in item.data.modules)
                            {
                                int index = 0;
                                foreach (var unit in module.units)
                                {
                                    data.Add(new MediaDataDescription
                                    {
                                        Name = unit.title,
                                        Url = $"https://learn.microsoft.com/en-us{unit.url}",
                                        MediaType = MediaSymbolType.MicrosoftLearn,
                                        ImageUrl = $"{symbol.Address}{module.iconUrl}",
                                        Order = index,
                                        ExtensionName = module.title
                                    });
                                    index++;
                                }
                            }
                        }
                        else
                        {
                            var resutl = await GetModuleArticles($"{symbol.Address}/hierarchy/modules/{item.id}?locale=en-us");
                            foreach (var r in resutl)
                            {
                                data.Add(new MediaDataDescription
                                {
                                    Name = r.Name,
                                    Url = r.Url,
                                    MediaType = MediaSymbolType.MicrosoftLearn,
                                    ImageUrl = r.ImageUrl,
                                    Order = r.Order,
                                    ExtensionName = r.ExtensionName
                                });
                            }

                        }
                    }
                }
                else
                {
                    var moduleResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleResponse>(content);
                    if (moduleResult.modules != null)
                    {
                        foreach (var module in moduleResult.modules)
                        {
                            int index = 0;
                            foreach (var item in module.units)
                            {
                                data.Add(new MediaDataDescription
                                {
                                    Name = item.title,
                                    Url = $"https://learn.microsoft.com/en-us{item.url}",
                                    MediaType = MediaSymbolType.MicrosoftLearn,
                                    Order = index,
                                    ExtensionName = module.achievement.title
                                });
                                index++;
                            }
                        }
                    }
                }

            }

            return Result(data);
        }

        private async Task<IList<MediaDataDescription>> GetModuleArticles(string url)
        {
            var data = new List<MediaDataDescription>();
            string content = await HttpHelper.GetUrlContentAsync(url, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(content))
            {
                var index = 0;
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.MicrosoftLearn.ToString());
                var module = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleItem>(content);

                if (module != null)
                {
                    foreach (var unit in module.units)
                    {

                        data.Add(new MediaDataDescription
                        {
                            Name = unit.title,
                            Url = $"https://learn.microsoft.com/en-us{unit.url}",
                            MediaType = MediaSymbolType.MicrosoftLearn,
                            Order = index,
                            ExtensionName = module.title
                        });
                        index++;
                    }

                }
            }

            return data;
        }

        private List<MediaDataDescription> RetrieveLearnPathArticle(string address)
        {
            var data = new List<MediaDataDescription>();
            var header = new Dictionary<string, string>() { { "accept-language", "en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7" } };
            string html = HttpHelper.GetUrlContentAsync(address, Encoding.GetEncoding("utf-8"), string.Empty, false, header).GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(html))
            {

                //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(SUBXPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"https://learn.microsoft.com/en-us/training{innerNode.GetAttributeValue("href", string.Empty).Replace("../../", "/")}";
                    var name = HttpUtility.HtmlDecode(ContentReplace(innerNode.InnerText).Replace(".", ""));

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        Url = url,
                        MediaType = MediaSymbolType.MicrosoftLearn,
                        Order = index
                    });
                    index++;

                }

            }
            return data;
        }

    }
}

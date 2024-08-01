using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class GeeksforgeeksPageDescription : BaseMediaPageDescription, IGrabArtcleContent, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='second']/div";
        private string XPATHExternal = @"//*[@class='second']/li";
        private string CONTENTXPATH = @"//*[@class='a-wrapper']/article";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public GeeksforgeeksPageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
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

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync2(MediaGetContext.Key);
            if (!string.IsNullOrEmpty(html))
            {

                //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Dzone.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {
                    var nameNode = categoryNode.SelectSingleNode(@".//h2");

                    var name = HttpUtility.HtmlDecode(ContentReplace(nameNode.InnerText).Replace(".", ""));
                    var childsNode = categoryNode.SelectNodes(@".//ul/li");
                    if (childsNode != null)
                    {

                        foreach (HtmlNode child in childsNode)
                        {
                            var hrefNode = child.SelectSingleNode(@".//a");
                            var url = $"{hrefNode.GetAttributeValue("href", string.Empty)}";
                            var childName = HttpUtility.HtmlDecode(ContentReplace(hrefNode.InnerText).Replace(".", ""));
                            data.Add(new MediaDataDescription
                            {
                                Name = childName,
                                Url = url,
                                MediaType = MediaSymbolType.Geeksforgeeks,
                                ExtensionName= name,
                                Order = index
                            });
                            index++;
                        }
                    }
                }

                var  categoryNodeExternal = rootNode.SelectNodes(XPATHExternal);

                if(categoryNodeExternal!=null)
                {
                    foreach(var item in categoryNodeExternal)
                    {
                        var hrefNode = item.SelectSingleNode(@".//a");

                        var name = HttpUtility.HtmlDecode(ContentReplace(hrefNode.InnerText).Replace(".", ""));
                        var url = $"{hrefNode.GetAttributeValue("href", string.Empty)}";

                        if (name.EndsWith("Tutorial"))
                        {
                            string htmlTutorial = await HttpHelper.GetUrlContentAsync2(url);
                            if (!string.IsNullOrEmpty(htmlTutorial))
                            {
                                HtmlDocument documentTutorial = new HtmlDocument();
                                documentTutorial.LoadHtml(htmlTutorial);
                                HtmlNode rootNodeTutorial = documentTutorial.DocumentNode;
                                HtmlNodeCollection categoryNodeListTutorial = rootNodeTutorial.SelectNodes(XPATHExternal);
                                if (categoryNodeListTutorial != null)
                                {
                                    foreach (var externalItem in categoryNodeListTutorial)
                                    {
                                        var externalHrefNode = externalItem.SelectSingleNode(@".//a");
                                        var nameExternal = HttpUtility.HtmlDecode(ContentReplace(externalHrefNode.InnerText).Replace(".", ""));
                                        var urlExternal = $"{externalHrefNode.GetAttributeValue("href", string.Empty)}";


                                        data.Add(new MediaDataDescription
                                        {
                                            Name = nameExternal,
                                            Url = urlExternal,
                                            MediaType = MediaSymbolType.Geeksforgeeks,
                                            ExtensionName = name,
                                            Order = index
                                        });
                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }

            }

            return Result(data);
        }

        public async Task<string> GetContentAsync(string address)
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
            var contentNode = rootNode.SelectSingleNode(CONTENTXPATH);

            var content = contentNode.InnerHtml;

            return content;
        }
    }
}

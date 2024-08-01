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
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class SimplifyingtheoryPageDescription : BaseMediaPageDescription, IGrabArtcleContent, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='entry-content']/div/div";
        private string PARENTXPATH = @"//*[@class='toc__part toc__part--full toc__parent']";
        private string CONTENTXPATH = @"//*[@class='entry-content']";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public SimplifyingtheoryPageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
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

        class TroubleshootItem
        {
            public string href { get; set; }
            public string toc_title { get; set; }

            public IList<TroubleshootItem> children { get; set; }
        }

        class TroubleshootList
        {
            public IList<TroubleshootItem> children { get; set; }
            public string href { get; set; }
            public string toc_title { get; set; }
        }


        class TroubleshootResponseItem
        {
            public IList<TroubleshootItem> items { get; set; }
            public string href { get; set; }
            public string toc_title { get; set; }
        }


        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {

                //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Dzone.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                var groupindex = 1;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {

                    var articleNodes = categoryNode.SelectNodes(@".//p");

                    foreach (HtmlNode articleNode in articleNodes)
                    {
                        var linkNode = articleNode.SelectSingleNode(@".//a");
                        var url = $"{linkNode.GetAttributeValue("href", string.Empty)}";
                        var name = HttpUtility.HtmlDecode(ContentReplace(linkNode.InnerText).Replace(".", ""));

                        data.Add(new MediaDataDescription
                        {
                            Name = name,
                            Url = url,
                            MediaType = MediaSymbolType.Simplifyingtheory,
                            Order = index,
                            ExtensionName= $"Module {groupindex}"
                        });
                        index++;

                    }
                    //var innerNode = categoryNode.SelectSingleNode(@".//header/h2/a");
                    //var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";
                    //var name = HttpUtility.HtmlDecode(ContentReplace(innerNode.InnerText).Replace(".", ""));

                    groupindex++;
                }
            }

            return Result(data);
        }

        private void AddItems(TroubleshootItem parent, string groupName, Action<MediaDataDescription> action)
        {
            int index = 1;

            if (parent.children != null)
            {
                foreach (var innerItem in parent.children)
                {
                    AddItems(innerItem, groupName, action);

                    //if (innerItem.children != null)
                    //{
                    //    foreach (var xitem in innerItem.children)
                    //    {
                    //        AddItems(xitem, action);
                    //        //action(new MediaDataDescription
                    //        //{
                    //        //    Name = xitem.toc_title,
                    //        //    Url = $@"{MediaGetContext.Key.Replace("/toc.json", "")}/{xitem.href}",
                    //        //    MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                    //        //    ExtensionName = parent.toc_title,
                    //        //    Order = index,
                    //        //});
                    //    }
                    //}
                    //else
                    //{
                    //    action(new MediaDataDescription
                    //    {
                    //        Name = innerItem.toc_title,
                    //        Url = $@"{MediaGetContext.Key.Replace("/toc.json", "")}/{innerItem.href}",
                    //        MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                    //        ExtensionName = parent.toc_title,
                    //        Order = index,
                    //    });


                    //    //data.Add(new MediaDataDescription
                    //    //{
                    //    //    Name = innerItem.toc_title,
                    //    //    Url = $@"{MediaGetContext.Key.Replace("/toc.json", "")}/{innerItem.href}",
                    //    //    MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                    //    //    ExtensionName = item.toc_title,
                    //    //    Order = index,
                    //    //});
                    //}
                    //index++;
                }
            }
            else
            {
                action(new MediaDataDescription
                {
                    Name = parent.toc_title,
                    Url = $@"{MediaGetContext.Key.Replace("/toc.json", "")}/{parent.href}",
                    MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                    ExtensionName = groupName,
                    Order = index,
                });

            }

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

            var content = contentNode?.InnerHtml??"unkown";

            return content;
        }
    }
}

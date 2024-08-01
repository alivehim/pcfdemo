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
using System.Threading.Tasks.Sources;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class MicrosofTroubleShootPageDescription : BaseMediaPageDescription, IGrabArtcleContent, IMediaPageDescriptor<MediaDataDescription>
    {
        //private string XPATH = @"//*[@class='column is-12 is-4-desktop']";
        //private string PARENTXPATH = @"//*[@class='toc__part toc__part--full toc__parent']";
        private string CONTENTXPATH = @"//*[@class='content ']";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public MicrosofTroubleShootPageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
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
            //string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            string html = await HttpHelper.GetUrlContentAsync2(MediaGetContext.Key);
            if (!string.IsNullOrEmpty(html))
            {

                var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<TroubleshootResponseItem>(html);


                var mc = Regex.Match(MediaGetContext.Key, @"toc.json(?<key>[\w\W]*)");
                var replace = "/toc.json";
                var endString = string.Empty;
                if (mc.Success)
                {
                    endString = mc.Groups["key"].Value;
                    replace = $"/{mc.Value}";
                }

                var index = 0;
                foreach (var item in jsonResult.items[0].children)
                {
                    if (item.children == null)
                    {
                        data.Add(new MediaDataDescription
                        {
                            Name = item.toc_title,
                            Url = $@"{MediaGetContext.Key.Replace(replace, "")}/{item.href}{endString}",
                            MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                            ExtensionName = item.toc_title,
                            Order = index,
                        });
                    }
                    else
                    {
                        AddItems(item, item.toc_title, replace, endString, (x) =>
                        {
                            data.Add(x);
                        });

                        //index = 0;
                        //foreach (var li in item.children)
                        //{
                        //    AddItems(li, (x) => {
                        //        data.Add(x);
                        //    });

                        //    //if (!string.IsNullOrEmpty(li.href))
                        //    //{
                        //    //    data.Add(new MediaDataDescription
                        //    //    {
                        //    //        Name = li.toc_title,
                        //    //        Url = $@"{MediaGetContext.Key.Replace("/toc.json", "")}/{li.href}",
                        //    //        MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                        //    //        ExtensionName = item.toc_title,
                        //    //        Order = index,
                        //    //    });
                        //    //}
                        //    //else
                        //    //{
                        //    //    if (li.children != null)
                        //    //    {
                        //    //        foreach (var innerItem in li.children)
                        //    //        {

                        //    //            AddItems(innerItem, (x) => {
                        //    //                data.Add(x);
                        //    //            });
                        //    //            //if (innerItem.children != null)
                        //    //            //{
                        //    //            //    foreach (var xitem in innerItem.children)
                        //    //            //    {
                        //    //            //        data.Add(new MediaDataDescription
                        //    //            //        {
                        //    //            //            Name = xitem.toc_title,
                        //    //            //            Url = $@"{MediaGetContext.Key.Replace("/toc.json", "")}/{xitem.href}",
                        //    //            //            MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                        //    //            //            ExtensionName = item.toc_title,
                        //    //            //            Order = index,
                        //    //            //        });
                        //    //            //    }
                        //    //            //}
                        //    //            //else
                        //    //            //{
                        //    //            //    data.Add(new MediaDataDescription
                        //    //            //    {
                        //    //            //        Name = innerItem.toc_title,
                        //    //            //        Url = $@"{MediaGetContext.Key.Replace("/toc.json", "")}/{innerItem.href}",
                        //    //            //        MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                        //    //            //        ExtensionName = item.toc_title,
                        //    //            //        Order = index,
                        //    //            //    });
                        //    //            //}

                        //    //        }
                        //    //    }
                        //    //}

                        //    //index++;
                        //}
                    }


                }
                ////var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
                //HtmlDocument document = new HtmlDocument();
                //document.LoadHtml(html);
                //HtmlNode rootNode = document.DocumentNode;
                //HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);


                ////data.AddRange(GetParentChapters(html));

                //foreach (HtmlNode categoryNode in categoryNodeList)
                //{

                //    var chapterName = categoryNode.SelectSingleNode(@".//div/h2").InnerText;

                //    var chapterList = categoryNode.SelectNodes(@".//div/ul/li");

                //    int index = 0;
                //    foreach (HtmlNode chapterNode in chapterList)
                //    {
                //        var hrefNode = chapterNode.SelectSingleNode(@".//a");
                //        var url = $"{hrefNode.GetAttributeValue("href", string.Empty)}";
                //        var name = HttpUtility.HtmlDecode(ContentReplace(hrefNode.InnerText).Replace(".", ""));

                //        data.Add(new MediaDataDescription
                //        {
                //            Name = name,
                //            Url = url,
                //            MediaType = MediaSymbolType.MicrosoftTroubleshoot,
                //            ExtensionName = chapterName,
                //            Order = index,
                //        });
                //        index++;
                //    }


                //}
            }

            return Result(data);
        }

        private void AddItems(TroubleshootItem parent, string groupName, string replace, string endstring, Action<MediaDataDescription> action)
        {
            int index = 1;

            if (parent.children != null)
            {
                foreach (var innerItem in parent.children)
                {
                    AddItems(innerItem, groupName, replace, endstring, action);

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
                    Url = $@"{MediaGetContext.Key.Replace(replace, "")}/{parent.href}{endstring}",
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

            var content = contentNode.InnerHtml;


            content = Regex.Replace(content, @"<ul class=""metadata page-metadata"">(((?!ul)[\s\S])*)</ul>", (mc) => { return string.Empty; });
            content = Regex.Replace(content, @"<div id=""user-feedback"">(((?!div)[\s\S])*)</div>", (mc) => { return string.Empty; });
            content = Regex.Replace(content, @"<div class=""xp-tag-hexagon"">(((?!div)[\s\S])*)</div>", (mc) => { return string.Empty; });
            content = Regex.Replace(content, @"<div id=""next-section"">(((?!div)[\s\S])*)</div>", (mc) => { return string.Empty; });
            content = Regex.Replace(content, @"<div id=""modular-content-container"">(((?!div)[\s\S])*)</div>", (mc) => { return string.Empty; });


            return content;
        }
    }
}

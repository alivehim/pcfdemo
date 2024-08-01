﻿using HtmlAgilityPack;
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
    public class OpenpressPageDescription : BaseMediaPageDescription, IGrabArtcleContent, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='toc__part toc__part--full']";
        private string CONTENTXPATH = @"//*[@id='content']";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public OpenpressPageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
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
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {

                //var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                foreach (HtmlNode categoryNode in categoryNodeList)
                {

                    var chapterName = categoryNode.SelectSingleNode(@".//div").InnerText;

                    var chapterList = categoryNode.SelectNodes(@".//ol/li");

                    int index = 0;
                    foreach (HtmlNode chapterNode in chapterList)
                    {
                        var hrefNode = chapterNode.SelectSingleNode(@".//div/p/a");
                        var url = $"{hrefNode.GetAttributeValue("href", string.Empty)}";
                        var name = HttpUtility.HtmlDecode(ContentReplace(hrefNode.InnerText).Replace(".", ""));

                        data.Add(new MediaDataDescription
                        {
                            Name = name,
                            Url = url,
                            MediaType = MediaSymbolType.Openpress,
                            ExtensionName = chapterName,
                            Order = index,
                        });
                        index++;
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

            //https://www.orcode.com/question/134170_ka8c59.html
            content = Regex.Replace(content, @"<sup>(.*?)</sup>", (mc) => { return string.Empty; });

            return content;
        }
    }
}
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Utilites;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule.MediaGet;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class XHOpenPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='video-elem']";
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public XHOpenPageDescription(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public string ShortIcon => "website.png";

        public override string GetNextAddress(string address, out int page)
        {
            throw new NotImplementedException();
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            throw new NotImplementedException();
        }


        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string html =await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {

                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.DomesticNineOne.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                var path = GetStoragePath();
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";
                    var previewurl = innerNode.GetAttributeValue("data-previewvideo", string.Empty);
                    var durationnode = categoryNode.SelectSingleNode(@".//a/small");
                    var duration = durationnode?.InnerText;
                    var namenode = categoryNode.SelectSingleNode(@".//a[2]");
                    var name = HttpUtility.HtmlDecode(ContentReplace(namenode.InnerText).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//a/div[2]");
                    var style = imageNode.GetAttributeValue("style", string.Empty);
                    var imageurl = string.Empty;
                    var mc = Regex.Match(style, @"'(?<key>.*?)'");
                    if (mc.Success)
                    {
                        imageurl = mc.Groups["key"].Value.ToString();
                    }

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = $"https:{imageurl}",
                        Url = $"{symbol.Address}{url}",
                        Duration = duration,
                        StoragePath = $"{path}\\{name}.mp4",
                        MediaType = MediaSymbolType.DomesticNineOne
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Utilites;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule.MediaGet;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class XHAPageDescriptor : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='thumb-list__item video-thumb role-pop']";
        public string ShortIcon => "website.png";

        public override string  GetNextAddress(string address, out int page)
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

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {
                    if (categoryNode.HasClass("right-rectangle"))
                    {
                        continue;
                    }


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    bool ishd = false;
                    if (innerNode.SelectNodes(".//i") != null)
                    {
                        ishd = true;
                    }
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";
                    var previewurl = innerNode.GetAttributeValue("data-previewvideo", string.Empty);
                    var durationnode = categoryNode.SelectSingleNode(@".//a/div");
                    var duration = durationnode?.InnerText;
                    var imageNode = categoryNode.SelectSingleNode(@".//a/img");
                    var name = ContentReplace(imageNode.GetAttributeValue("alt", string.Empty)).Replace(".", "");
                    var imageurl = imageNode.GetAttributeValue("src", string.Empty);


                    var filename = FileHelper.GetValideName(name.Trim().Replace(" ", "-").Replace(".", "-"), false);
                    data.Add(new MediaDataDescription
                    {
                        Name = filename,
                        ImageUrl = imageurl,
                        Url=url,
                        Duration = duration,
                        StoragePath = GetStoragePath(),
                        IsHD = ishd,
                        MediaType = UtilityTools.Core.Models.MediaSymbolType.XHA
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}

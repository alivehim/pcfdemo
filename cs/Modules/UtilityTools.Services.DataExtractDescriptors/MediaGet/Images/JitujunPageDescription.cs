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
    public class JitujunPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IImageCollector
    {
        private string XPATH = @"//*[@class='posts-item card ajax-item style3']";
        private string IMAGEXPATH = @"//*[@class='theme-box wp-posts-content']/p";
        //private string IMAGEXPATH = @"//*[@class='theme-box wp-posts-content']/p[7]/img";
        //private string IMAGEXPATH2 = @"//*[@class='theme-box wp-posts-content']/p[3]/img";
        //private string IMAGEXPATH3 = @"//*[@class='theme-box wp-posts-content']/p[4]/img";
        //private string IMAGEXPATH4 = @"//*[@class='theme-box wp-posts-content']/p[5]/img";
        //private string IMAGEXPATH5 = @"//*[@class='theme-box wp-posts-content']/p[6]/img";
        public string ShortIcon => "website.png";
        private readonly IEventAggregator eventAggregator;
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public JitujunPageDescription(IEventAggregator eventAggregator, IMediaSymbolDBService mediaSymbolDBService)
        {
            this.eventAggregator = eventAggregator;
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address,out int page)
        {
            int localpage = 0;
            var result= Regex.Replace(address, @"page/(?<key>[\d]*)", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"page/{localpage + 1}";
            });

            page = localpage + 1;
            return result;

        }

        public override string GetProceedingAddress(string address,out int page)
        {
            int localpage = 0;

            var result= Regex.Replace(address, @"page/(?<key>[\d]*)", (mc) =>
            {
               localpage = int.Parse(mc.Groups["key"].Value);
                return $"page/{localpage - 1}";
            });

            page = localpage - 1; return result;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//div/a");
                    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                    var namenode = categoryNode.SelectSingleNode(@".//div[2]/h2/a");
                    var name = HttpUtility.HtmlDecode(ContentReplace(namenode.InnerText).Replace(".", ""));

                    var imageNode = categoryNode.SelectSingleNode(@".//div/a/img");
                    var imageurl = $"{imageNode?.GetAttributeValue("data-src", string.Empty)}";

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        Order = index,
                        Url = url,
                        MediaType = MediaSymbolType.Jitujun,
                        StoragePath = $"{Settings.Current.MangaFolder}\\{name}"
                    });
                    index++;
                }
            }

            return Result(data);
        }

        public async Task<IList<MediaDataDescription>> GetImagesAsync(string address)
        {
            string html = await HttpHelper.GetUrlContentAsync(address, Encoding.GetEncoding("utf-8"));
            var result = new List<MediaDataDescription>();

            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(IMAGEXPATH);
                int index = 0;

                foreach(var categoryNode in categoryNodeList)
                {
                    var imagesNode = categoryNode.SelectNodes(@".//img");

                    if(imagesNode!=null)
                    {
                        foreach (var imageNode in imagesNode)
                        {
                            var url = imageNode.GetAttributeValue("src", string.Empty);

                            result.Add(new MediaDataDescription
                            {
                                ImageUrl = url,
                                Order = index,
                                Url = url,
                            });
                            index++;
                        }
                    }
                   
                }
                //if (categoryNodeList == null)
                //{
                //    categoryNodeList = rootNode.SelectNodes(IMAGEXPATH2);
                //    foreach (HtmlNode categoryNode in categoryNodeList)
                //    {


                //        var url = categoryNode.GetAttributeValue("src", string.Empty);

                //        result.Add(new MediaDataDescription
                //        {
                //            ImageUrl = url,
                //            Order = index,
                //            Url = url,
                //        });
                //        index++;
                //    }

                //    categoryNodeList = rootNode.SelectNodes(IMAGEXPATH3);
                //    foreach (HtmlNode categoryNode in categoryNodeList)
                //    {


                //        var url = categoryNode.GetAttributeValue("src", string.Empty);

                //        result.Add(new MediaDataDescription
                //        {
                //            ImageUrl = url,
                //            Order = index,
                //            Url = url,
                //        });
                //        index++;
                //    }
                //}

                //if (categoryNodeList == null)
                //{
                //    categoryNodeList = rootNode.SelectNodes(IMAGEXPATH4);
                //    foreach (HtmlNode categoryNode in categoryNodeList)
                //    {


                //        var url = categoryNode.GetAttributeValue("src", string.Empty);

                //        result.Add(new MediaDataDescription
                //        {
                //            ImageUrl = url,
                //            Order = index,
                //            Url = url,
                //        });
                //        index++;
                //    }

                //    categoryNodeList = rootNode.SelectNodes(IMAGEXPATH5);
                //    foreach (HtmlNode categoryNode in categoryNodeList)
                //    {


                //        var url = categoryNode.GetAttributeValue("src", string.Empty);

                //        result.Add(new MediaDataDescription
                //        {
                //            ImageUrl = url,
                //            Order = index,
                //            Url = url,
                //        });
                //        index++;
                //    }
                //}

                //if (categoryNodeList == null)
                //{
                //    foreach (HtmlNode categoryNode in categoryNodeList)
                //    {


                //        var url = categoryNode.GetAttributeValue("src", string.Empty);

                //        result.Add(new MediaDataDescription
                //        {
                //            ImageUrl = url,
                //            Order = index,
                //            Url = url,
                //        });
                //        index++;
                //    }
                //}
               

            }
            return result;
        }
    }
}

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
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class BlackYPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        //private string XPATH = @"//*[@class='Grid__Item-f0cb34-1 dSIsBc']";
        public string ShortIcon => "website.png";

        public override string GetNextAddress(string address, out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetProceedingAddressWithEqualSign(address, out page);    
        }

        public class PagePropsContainer
        {
            public PageProps pageProps { get; set; }

        }

        public class PageProps
        {
            public IList<PageEdge> edges { get; set; }
            public IList<PageVideo> videos { get; set; }
        }

        public class PageEdge
        {
            public PageEdgeNode node { get; set; }
        }

        public class PageVideo
        {
            public string id { get; set; }
            public string title { get; set; }

            public PageEdgeNodeImage images { get; set; }

        }


        public class PageEdgeNode
        {
            public string id { get; set; }
            public string title { get; set; }

            public PageEdgeNodeImage images { get; set; }
        }

        public class PageEdgeNodeImage
        {
            public IList<PageEdgeNodeImageDescription> listing { get; set; }
        }

        public class PageEdgeNodeImageDescription
        {
            public string src { get; set; }
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagePropsContainer>(html);

                int index = 0;
                if (result != null)
                {
                    if (result.pageProps.edges != null)
                    {

                        foreach (var item in result.pageProps.edges)
                        {

                            data.Add(new MediaDataDescription
                            {
                                Name = GetName(item.node.id),
                                ImageUrl = item.node.images.listing[0].src,
                                Url = string.Empty,
                                MediaType = MediaSymbolType.BlackY,
                                Order = index
                            });
                            index++;

                        }
                    }
                    else
                    {
                        foreach (var item in result.pageProps.videos)
                        {

                            data.Add(new MediaDataDescription
                            {
                                Name = GetName(item.id),
                                ImageUrl = item.images.listing[0].src,
                                Url = string.Empty,
                                MediaType = MediaSymbolType.BlackY,
                                Order = index
                            });
                            index++;

                        }
                    }
                }

                //HtmlDocument document = new HtmlDocument();
                //document.LoadHtml(html);
                //HtmlNode rootNode = document.DocumentNode;
                //HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                //foreach (HtmlNode categoryNode in categoryNodeList)
                //{


                //    var innerNode = categoryNode.SelectSingleNode(@".//div/div[2]/div/a");
                //    var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                //    var name = innerNode.InnerText;

                //    var imageNode = categoryNode.SelectSingleNode(@".//div/div[1]/div/picture/img");
                //    var imageurl = imageNode.GetAttributeValue("src", string.Empty);


                //}
            }

            return Result(data);
        }

        private string GetName(string name)
        {

            return name.Replace("blacked:", "").Replace("blackedraw:", "").Replace("-", " ");
        }
    }
}

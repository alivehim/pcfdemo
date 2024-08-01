using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using UtilityTools.CEF;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Infrastructure;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class JAVDBPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='item']";
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;
        private readonly IHttpService httpService;

        public JAVDBPageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator, IHttpService httpService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.eventAggregator = eventAggregator;
            this.httpService = httpService;
        }

        public override string GetNextAddress(string address, out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            //string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            var html = string.Empty;
            var dispatcher = Application.Current.Dispatcher;
            IDictionary<string, byte[]> imagesources = null;
            await dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                var form = new FetchSrcWindow(MediaGetContext.Key);

                //form.Visibility = Visibility.Hidden;
                form.ShowDialog();

                html = form.HtmlSource;

                imagesources = form.ImageSources;
            }));
            //string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.JAVDB.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";

                    var nameNode = categoryNode.SelectSingleNode(@".//a/div[2]/strong");
                    var nameNode1 = categoryNode.SelectSingleNode(@".//a/div[2]");
                    //var nameunhandle = $"{innerNode.GetAttributeValue("title", string.Empty)}";
                    var name = HttpUtility.HtmlDecode(ContentReplace(nameNode.InnerText).Replace(".", ""));
                    var imageNode = categoryNode.SelectSingleNode(@".//a/div/img");
                    var imageurl = imageNode.GetAttributeValue("src", string.Empty);

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        ImageUrl = imageurl,
                        ExtensionName = nameNode1.InnerText,
                        Url = url,
                        Order = index,
                        MediaType = MediaSymbolType.JAVDB,
                        ImageSource = imagesources.FirstOrDefault(p => p.Key == imageurl).Value
                    });
                    index++;

                }
            }

            return Result(data);
        }
    }
}

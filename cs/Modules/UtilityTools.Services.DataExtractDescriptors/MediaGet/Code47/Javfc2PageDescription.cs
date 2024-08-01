using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Threading;
using UtilityTools.CEF;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class Javfc2PageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        private string XPATH = @"//*[@class='videos-list']/article";
        private string XPATH2 = @"//*[@id='main']/div/article";
        public string ShortIcon => "website.png";

        public override string GetNextAddress(string address, out int page)
        {
            return GetNextAddressWithPage(address, out page);
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetProceedingAddressWithPage(address, out page);
        }

        private readonly IHttpService httpService;

        public Javfc2PageDescription(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var data = new List<MediaDataDescription>();
            var html = string.Empty;
            //var dispatcher = Application.Current.Dispatcher;
            //await dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            // {
            //     var form = new FetchSrcWindow(MediaGetContext.Key);

            //     //form.Visibility = Visibility.Hidden;
            //     form.ShowDialog();

            //     html = form.HtmlSource;
            // }));

            //return Result(data);

            var result = await httpService.GetHtmlSourceAsync(MediaGetContext.Key);
            html = result;
            int index = 0;
            //var html= 
            if (!string.IsNullOrEmpty(html))
            {

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                if (categoryNodeList != null)
                {
                   
                    foreach (HtmlNode categoryNode in categoryNodeList)
                    {


                        var innerNode = categoryNode.SelectSingleNode(@".//a");
                        var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                        var nameunhandle = $"{innerNode.GetAttributeValue("title", string.Empty)}";
                        var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle).Replace(".", ""));
                        var imageNode = categoryNode.SelectSingleNode(@".//a/div/div/img");
                        var imageurl = imageNode?.GetAttributeValue("src", string.Empty);
                        ToolsContext.Current.PostMessage(imageurl);
                        data.Add(new MediaDataDescription
                        {
                            Name = name,
                            ImageUrl = imageurl ?? "",
                            Url = url,
                            Order = index,
                            MediaType = MediaSymbolType.Javfc2
                        });
                        index++;
                    }
                }

            }

            if (!string.IsNullOrEmpty(html))
            {

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH2);
                if (categoryNodeList != null)
                {

                    foreach (HtmlNode categoryNode in categoryNodeList)
                    {


                        var innerNode = categoryNode.SelectSingleNode(@".//a");
                        var url = $"{innerNode.GetAttributeValue("href", string.Empty)}";

                        var nameunhandle = $"{innerNode.GetAttributeValue("title", string.Empty)}";
                        var name = HttpUtility.HtmlDecode(ContentReplace(nameunhandle).Replace(".", ""));
                        var imageNode = categoryNode.SelectSingleNode(@".//a/div/div/img");
                        var imageurl = imageNode?.GetAttributeValue("src", string.Empty);

                        data.Add(new MediaDataDescription
                        {
                            Name = name,
                            ImageUrl = imageurl ?? "",
                            Url = url,
                            Order = index,
                            MediaType = MediaSymbolType.Javfc2
                        });
                        index++;
                    }
                }

            }

            return Result(data);
        }
    }
}

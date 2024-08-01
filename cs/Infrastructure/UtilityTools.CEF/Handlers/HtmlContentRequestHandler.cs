using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;

namespace UtilityTools.CEF.Handlers
{
    //public class MonitorNetworkRequestHandler : CefSharp.Handler.ResourceRequestHandler
    //{
    //    //public override void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
    //    //{

    //    //}

    //    //public override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
    //    //{
    //    //    ToolsContext.Current.PostMessage("abc", MessageOwner.PowerPlatform);
    //    //    return CefReturnValue.Continue;
    //    //}
    //}

    public class ImageResoureRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            //return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
            return CefReturnValue.Cancel;
        }

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            //You can now get the data from the stream
            //var bytes = memoryStream.ToArray();

            //if (response.Charset == "utf-8")
            //{
            //    //var str = System.Text.Encoding.UTF8.GetString(bytes);

            //}
            //else
            //{
            //    //Deal with different encoding here
            //}
        }
    }

    public class HtmlContentRequestHandler : CefSharp.Handler.RequestHandler
    {
        private string url;

        public HtmlContentRequestHandler(string url)
        {
            this.url = url;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //Console.WriteLine("In GetResourceRequestHandler : " + request.Url);
            //Only intercept specific Url's
            if (!request.Url.Contains(url)
                )
            {
                return new ImageResoureRequestHandler();
            }

            ToolsContext.Current.PostMessage(request.Url);
            //Default behaviour, url will be loaded normally.
            return null;

        }

    }
}

using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

    public class JAVImageResoureRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        IImageContainer imageContainer;

        private  System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        //Dictionary<string, ImageSource> dic = new Dictionary<string, ImageSource>();

        public JAVImageResoureRequestHandler(IImageContainer imageContainer)
        {
            this.imageContainer = imageContainer;
        }

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            //return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
            return CefReturnValue.Continue;
        }

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            var bytebuffer = memoryStream.ToArray();
            //var dataLength = data.Length;
            //NOTE: You may need to use a different encoding depending on the request

            //var image = new BitmapImage();

            //MemoryStream ms = new MemoryStream(bytebuffer);

            //image.BeginInit();
            //memoryStream.Seek(0, SeekOrigin.Begin);

            //image.StreamSource = ms;
            //image.EndInit();

            imageContainer.ImageSources.Add(request.Url, bytebuffer);
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

        protected override void Dispose()
        {
            memoryStream?.Dispose();
            memoryStream = null;

            base.Dispose();
        }
    }

    public class JAVRequestHandler : CefSharp.Handler.RequestHandler
    {
        IImageContainer imageContainer;
        public JAVRequestHandler(IImageContainer imageContainer)
        {
            this.imageContainer = imageContainer;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //Console.WriteLine("In GetResourceRequestHandler : " + request.Url);
            //Only intercept specific Url's
            if (request.Url.Contains("jpg")
                )
            {
                return new JAVImageResoureRequestHandler(imageContainer);
            }

            ToolsContext.Current.PostMessage(request.Url);
            //Default behaviour, url will be loaded normally.
            return null;

        }

    }
}

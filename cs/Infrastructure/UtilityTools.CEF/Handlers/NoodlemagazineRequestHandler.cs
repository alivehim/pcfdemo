using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;

namespace UtilityTools.CEF.Handlers
{

    public class NoodlemagazineRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        private readonly ICEFCallback cefCallback;

        public NoodlemagazineRequestHandler(ICEFCallback cefCallback)
        {
            this.cefCallback = cefCallback;
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

        private class StreamSource
        {
            public string file { get; set; }
            public string label { get; set; }
            public string type { get; set; }
        }

        private class StreamList
        {
            public IList<StreamSource> sources { get; set; }
        }


        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            //You can now get the data from the stream
            var bytes = memoryStream.ToArray();

            var str = System.Text.Encoding.UTF8.GetString(bytes);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<StreamList>(str);

            cefCallback.Action(obj?.sources.FirstOrDefault(p => p.label == "720")?.file);
        }
    }

    public class NoodlemagazineHandler : CefSharp.Handler.RequestHandler
    {
        //private string url;

        private readonly ICEFCallback cefCallback;
        public NoodlemagazineHandler(ICEFCallback cefCallback)
        {
            this.cefCallback = cefCallback;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //Console.WriteLine("In GetResourceRequestHandler : " + request.Url);
            //Only intercept specific Url's
            if (request.Url.Contains("/playlist/")
                )
            {
                return new NoodlemagazineRequestHandler(cefCallback);
            }

            return null;

        }

    }
}

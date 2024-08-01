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

    public class MyCustomResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            ToolsContext.Current.PostMessage($"{request.Method} {request.Url}", MessageOwner.PowerPlatform);

            if ((request.Url.StartsWith($"{Settings.Current.D365ResourceUrl}/api/data/v9.0/sdkmessages") || request.Url.StartsWith("https://asia.api.flow.microsoft.com/providers/Microsoft.ProcessSimple/environments") || request.Url.StartsWith("https://asia.api.powerapps.com/api/invoke")))
            {
                if (request.Headers["Authorization"] != null && request.Url.StartsWith($"{Settings.Current.D365ResourceUrl}/api/data/v9.0/sdkmessages"))
                {
                    ToolsContext.Current.PostMessage(request.Headers["Authorization"], MessageOwner.PowerPlatform);

                    if (Settings.Current.D365AccessToken != request.Headers["Authorization"])
                    {

                        Settings.Current.D365AccessToken = request.Headers["Authorization"];
                        Settings.Current.Save(nameof(Settings.Current.D365AccessToken));
                    }
                }
                else if (request.Headers["Authorization"] != null && (request.Url.StartsWith("https://asia.api.flow.microsoft.com/providers/Microsoft.ProcessSimple/environments") || request.Url.StartsWith("https://asia.api.powerapps.com/api/invoke")))
                {
                    ToolsContext.Current.PostMessage(request.Headers["Authorization"], MessageOwner.PowerPlatform);

                    if (Settings.Current.FlowToken != request.Headers["Authorization"])
                    {

                        Settings.Current.FlowToken = request.Headers["Authorization"];
                        Settings.Current.Save(nameof(Settings.Current.FlowToken));
                    }
                }
            }
            return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
        }

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            //You can now get the data from the stream
            var bytes = memoryStream.ToArray();

            if (response.Charset == "utf-8")
            {
                var str = System.Text.Encoding.UTF8.GetString(bytes);

            }
            else
            {
                //Deal with different encoding here
            }
        }
    }

    public class MonitorNetworkRequestHandler : CefSharp.Handler.RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //Console.WriteLine("In GetResourceRequestHandler : " + request.Url);
            //Only intercept specific Url's
            if (!request.Url.EndsWith(".js") && (request.Url.StartsWith($"{Settings.Current.D365ResourceUrl}/api/data/v9.0/sdkmessages") || request.Url.StartsWith("https://asia.api.flow.microsoft.com/providers/Microsoft.ProcessSimple/environments") || request.Url.StartsWith("https://asia.api.powerapps.com/api/invoke")))
            {
                return new MyCustomResourceRequestHandler();
            }
            //Default behaviour, url will be loaded normally.
            return null;

        }

    }
}

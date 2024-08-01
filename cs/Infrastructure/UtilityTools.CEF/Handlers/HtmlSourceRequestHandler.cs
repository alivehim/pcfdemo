using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityTools.CEF.Handlers
{
    internal class HtmlSourceRequestHandler: DefaultRequestHandler
    {




        public override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {

            Uri url;
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out url) == false)
            {
                return CefReturnValue.Cancel;
            }


            var extension = url.ToString().ToLower();
            if (request.ResourceType == ResourceType.Image
                || extension.EndsWith(".jpg")
                || extension.EndsWith(".png")
                || extension.EndsWith(".gif")
                || extension.EndsWith(".jpeg")
                || extension.EndsWith(".css")
                )
            {
                System.Diagnostics.Debug.WriteLine(url);//打印
                return CefReturnValue.Cancel;
            }

            if (request.ResourceType == ResourceType.Script || request.ResourceType == ResourceType.Stylesheet)
            {
                return CefReturnValue.Cancel;
            }

            if (request.ResourceType == ResourceType.SubFrame)
            {
                return CefReturnValue.Cancel;
            }
            //return CefReturnValue.Continue;

      
            return CefReturnValue.Continue;
        }

        public override IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return null;
        }


        public override void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
        }
    }
}

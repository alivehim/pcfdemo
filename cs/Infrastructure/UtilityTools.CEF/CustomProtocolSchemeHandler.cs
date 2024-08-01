using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityTools.CEF
{
    public class CustomProtocolSchemeHandler : ResourceHandler
    {
        public CustomProtocolSchemeHandler()
        {
        }

        //public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        //{
        //    return true;
        //}

        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            return CefReturnValue.ContinueAsync;
        }
    }

    public class CustomProtocolSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "customFileProtocol";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new CustomProtocolSchemeHandler();
        }
    }
}

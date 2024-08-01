using CefSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.CEF.Extensions
{
    internal static class WebBrowserExtension
    {
        public async static Task<CookieContainer> GetCookieContainer(this IWebBrowser browser, string url)
        {
            var cookieManager = browser.GetCookieManager();
            var list = await cookieManager.VisitUrlCookiesAsync(url, true);
            CookieContainer cc = new CookieContainer();
            if(list != null)
            {
                list.ForEach(p =>
                {
                    //var ccsap = new System.Net.Cookie()
                    //{
                    //    Name = p.Name,
                    //    Value = p.Value,
                    //    Domain = p.Domain,
                    //    Path = p.Path,
                    //};
                    var ccsap = new System.Net.Cookie();

                    ccsap.Name = p.Name;
                    ccsap.Value = p.Value.Replace(",", "%2C");
                    ccsap.Domain = p.Domain;
                    ccsap.Path = p.Path;

                    if (p.Expires.HasValue)
                    {
                        ccsap.Expires = p.Expires.Value;
                    }
                    cc.Add(ccsap);
                });
            }
          

            return cc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.CEF;
using UtilityTools.CEF.WebLoaders;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure;

namespace UtilityTools.Services.Infrastructure
{
    public class HttpService : IHttpService
    {
        public async Task<(string, CookieContainer)> GetPageSourceAsync(string uriString)
        {
            var cc = new CookieContainer();
            var loader = new DefaultWebLoader();
            try
            {
                bool result = true;
                int timeout = 3*60 * 1000;
                RegisteredWaitHandle waithandle = ThreadPool.RegisterWaitForSingleObject(loader.TimeoutEvent, (o, t) =>
                {
                    if (t)
                    {
                        string s = (string)o;
                        loader.Stop();
                        result = false;
                    }

                }, "state", timeout, true);

                if (await loader.Start(uriString))
                {
                    result = true;

                     cc = await loader.GetCookieAsync(new Uri(uriString).Host);

                    loader.Stop();
                    waithandle.Unregister(loader.TimeoutEvent);
                }
                else
                {

                }


                if (result)
                {
                    return (loader.Source,cc);
                }


                return (string.Empty,null);

            }
            catch (Exception ex)
            {
                ToolsContext.Current.PostMessage(ex.ToString());
                return (string.Empty, null);
            }
            finally
            {
                loader.Dispose();
            }
        }


        public async Task<string> GetHtmlSourceAsync(string uriString)
        {
            var cc = new CookieContainer();
            var loader = new HtmlContentWebLoader();
            try
            {
                bool result = true;
                int timeout = 3 * 60 * 1000;
                RegisteredWaitHandle waithandle = ThreadPool.RegisterWaitForSingleObject(loader.TimeoutEvent, (o, t) =>
                {
                    if (t)
                    {
                        string s = (string)o;
                        loader.Stop();
                        result = false;
                    }

                }, "state", timeout, true);

                if (await loader.Start(uriString))
                {
                    result = true;

                    cc = await loader.GetCookieAsync(new Uri(uriString).Host);

                    loader.Stop();
                    waithandle.Unregister(loader.TimeoutEvent);
                }
                else
                {

                }


                if (result)
                {
                    return loader.Source;
                }


                return string.Empty;

            }
            catch (Exception ex)
            {
                ToolsContext.Current.PostMessage(ex.ToString());
                return string.Empty;
            }
            finally
            {
                loader.Dispose();
            }
        }

        public Task<string> GetUrlContentAsync(string uriString, Encoding encoding)
        {
            return HttpHelper.GetUrlContentAsync(uriString, encoding);
        }

        public Task<string> GetUrlContentAsync(string uriString, Encoding encoding,string Referer)
        {
            return HttpHelper.GetUrlContentAsync(uriString, encoding,"",false,null,Referer);
        }
    }
}

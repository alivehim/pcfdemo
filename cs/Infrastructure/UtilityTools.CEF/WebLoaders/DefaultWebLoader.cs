using CefSharp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UtilityTools.Core.Models;
using CefSharp.OffScreen;
using UtilityTools.CEF.Handlers;
using UtilityTools.CEF.Extensions;

namespace UtilityTools.CEF
{
    public class DefaultWebLoader : IWebLoader
    {
        #region Fields

        private ChromiumWebBrowser browser = null;
        private ILoggerProvider logger;
        private AutoResetEvent resouseUrlDetected = new AutoResetEvent(false);
        private AutoResetEvent timeoutEvent = new AutoResetEvent(false);

        TaskCompletionSource<bool> loadcompletedSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        private string url;
        #endregion

        private string downloadId { get; set; }

        #region Props

        public string Source
        {
            get; set;
        }


        public bool IsValidate { get; set; }



        public AutoResetEvent ResouseUrlDetected => resouseUrlDetected;

        public AutoResetEvent TimeoutEvent => timeoutEvent;

        public string VideoString { get; set; }


        #endregion

        public string OutputMessage { get; set; }
        public string StatusMessage { get; set; }

        public string ErrorMessage { get; set; }

        public DefaultWebLoader()
        {
        }

        public Task LoadPageAsync(string address = null)
        {
            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    //Important that the continuation runs async using TaskCreationOptions.RunContinuationsAsynchronously
                    loadcompletedSource.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;


            return loadcompletedSource.Task;

        }

        public async Task<bool> Start(string url)
        {
            this.url = url;
            return await MainAsync("cachePath1", 1.0);
        }

        public void Stop()
        {
            loadcompletedSource.TrySetResult(true);
            Dispose(true);

        }
        private async Task<bool> MainAsync(string cachePath, double zoomLevel)
        {
            var browserSettings = new BrowserSettings() { ImageLoading = CefState.Disabled };
            //Reduce rendering speed to one frame per second so it's easier to take screen shots
            var requestContextSettings = new RequestContextSettings { /*CachePath = cachePath */};

            // RequestContext can be shared between browser instances and allows for custom settings
            // e.g. CachePath
            var requestContext = new RequestContext(requestContextSettings);
            browser = new ChromiumWebBrowser(url, browserSettings, requestContext);
            //browser = new ChromiumWebBrowser("_blank");
            if (zoomLevel > 1)
            {
                browser.FrameLoadStart += (s, argsi) =>
                {
                    var b = (ChromiumWebBrowser)s;
                    if (argsi.Frame.IsMain)
                    {
                        b.SetZoomLevel(zoomLevel);
                    }
                };
            }

            browser.ConsoleMessage += OnWebBrowserConsoleMessage;
            browser.StatusMessage += OnWebBrowserStatusMessage;
            browser.LoadError += OnWebBrowserLoadError;

            browser.RequestHandler = new HtmlSourceRequestHandler();

            browser.FrameLoadEnd += Browser_FrameLoadEnd;


            //logger.Debug("加载页面......");
            await LoadPageAsync(url);
            Console.WriteLine("加载完成");

            try
            {
                if (!string.IsNullOrEmpty(Source = await browser.GetSourceAsync()))
                {
                    IsValidate = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return IsValidate;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                browser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    Source = taskHtml.Result;
                });
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;

        }

        private void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            StatusMessage = e.Value;
        }

        private void OnWebBrowserLoadError(object sender, LoadErrorEventArgs args)
        {
            ErrorMessage = $"{args.ErrorCode}--{args.ErrorText}";
        }


        public async Task<CookieContainer> GetCookieAsync(string domain)
        {
            return await browser.GetCookieContainer(domain);
        }

        ~DefaultWebLoader()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {
                // 释放 托管资源 
                if (browser != null)
                {
                    browser.Dispose();
                }
            }

            if (disposing)
                GC.SuppressFinalize(this);

        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}

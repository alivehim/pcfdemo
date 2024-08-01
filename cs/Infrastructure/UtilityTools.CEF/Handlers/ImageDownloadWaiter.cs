//using CefSharp;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UtilityTools.CEF.Handlers
//{
//    public class ImageDownloadWaiter : ILifeSpanHandler
//    {
//        private readonly List<TaskCompletionSource<bool>> _imageDownloadTasks = new List<TaskCompletionSource<bool>>();

//        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
//        {
//            // 默认行为，允许关闭
//            return true;
//        }

//        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
//        {
//            // 在新页面加载后的操作
//            browser.ResourceResponseFilterFactory = new ImageResourceResponseFilterFactory(this);
//        }

//        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
//        {
//            // 所有图片下载任务完成
//            Task.WhenAll(_imageDownloadTasks.ToArray()).Wait();
//            Console.WriteLine("所有图片下载完毕");
//        }

//        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
//        {
//            throw new NotImplementedException();
//        }

//        public class ImageResourceResponseFilterFactory : IResourceResponseFilterFactory
//        {
//            private readonly ImageDownloadWaiter _parent;

//            public ImageResourceResponseFilterFactory(ImageDownloadWaiter parent)
//            {
//                _parent = parent;
//            }

//            public IResourceResponseFilter Create(IBrowser browser, IFrame frame)
//            {
//                return new ImageResourceResponseFilter(_parent);
//            }
//        }

//        public class ImageResourceResponseFilter : IResourceResponseFilter
//        {
//            private readonly ImageDownloadWaiter _parent;
//            private bool _isImage;

//            public ImageResourceResponseFilter(ImageDownloadWaiter parent)
//            {
//                _parent = parent;
//            }

//            public void Dispose()
//            {
//            }

//            public FilterStatus Ifilter(Stream dataIn, out long dataInRead, SkipStream dataInSkip, out long dataInSkipped, Stream dataOut, out long dataOutWritten)
//            {
//                dataInRead = 0;
//                dataInSkipped = 0;
//                dataOutWritten = 0;
//                return FilterStatus.Drain;
//            }

//            public FilterStatus Response(IResponse response, out long responseLength, out long responseSkipped)
//            {
//                var contentType = response.Headers["Content-Type"].ToLower();
//                _isImage = contentType.Contains("image");
//                responseLength = 0;
//                responseSkipped = 0;
//                return FilterStatus.Continue;
//            }

//            public bool Filter(IResponse response, out long responseLength, ICallback callback)
//            {
//                if (_isImage)
//                {
//                    var tcs = new TaskCompletionSource<bool>();
//                    _parent._imageDownloadTasks.Add(tcs);
//                    responseLength = 0;
//                    callback.Continue();
//                    return true;
//                }
//                responseLength = response.ResponseLength;
//                return false;
//            }
//        }
//    }

//}

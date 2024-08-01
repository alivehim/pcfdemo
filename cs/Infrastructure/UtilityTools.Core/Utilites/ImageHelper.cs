using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure;
using System.Net.Http;
using System.Net.Security;

namespace UtilityTools.Core.Utilites
{
    public static class ImageHelper
    {
        private static  HttpClient _httpClient;

        static ImageHelper()
        {
            var sslOptions = new SslClientAuthenticationOptions
            {
                // Leave certs unvalidated for debugging
                RemoteCertificateValidationCallback = delegate { return true; },
            };

            var handler = new SocketsHttpHandler()
            {
                UseProxy = false,
                Proxy = null,
                SslOptions = sslOptions,
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
            };
            string customUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", customUserAgent);
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => cert.Verify(); //{ return true; };
        }
        public static async Task<ImageSource> DownloadFileAsync(string uri)
        {
            Uri uriResult;

            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                throw new InvalidOperationException("URI is invalid.");

        
            byte[] bytebuffer = await _httpClient.GetByteArrayAsync(uri);
           
            var image = new BitmapImage();

            MemoryStream memoryStream = new MemoryStream(bytebuffer);

            image.BeginInit();
            memoryStream.Seek(0, SeekOrigin.Begin);

            image.StreamSource = memoryStream;
            image.EndInit();

            return image;

        }


        /// <summary>
        /// https://social.msdn.microsoft.com/Forums/windowsapps/en-US/aba468e9-aae4-4026-a8b4-5bfa5c949c01/uwprs21703deleting-files-used-by-uwp-image-control
        /// </summary>
        /// <param name="pngName"></param>
        /// <returns></returns>
        public static ImageSource GetImageSourceFromPath(string path)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(path))
                {
                    MemoryStream ms = new MemoryStream();
                    fileStream.CopyTo(ms);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    return bitmap;
                }


            }
            catch (FileNotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        public static ImageSource GetDefaultImageSourceFormName(string fold, string name)
        {
            var pathService = ToolsContext.Current.UnityContainer.ResolveService<IPathService>();
            var folder = pathService.DefaultPngsLocation.GetDirectories().FirstOrDefault(p => p.Name == fold);
            var pngfile = folder.GetFiles().FirstOrDefault(p => p.Name == name);
            return GetImageSourceFromPath(pngfile.FullName);
        }

        public static ImageSource GetCustomImageSourceFormName(string name)
        {
            var pathService = ToolsContext.Current.UnityContainer.ResolveService<IPathService>();

            var files = pathService.CustomPngsLocation.GetFiles();
            if (files.Any(p => p.Name.Contains(name)))
            {
                var pngfile = files.First(p => p.Name.Contains(name));

                return GetImageSourceFromPath(pngfile.FullName);
            }
            return null;
        }


        public static ImageSource DownloadImage(string url)
        {
            var image = new BitmapImage();
            int BytesToRead = 100;

            WebRequest request = WebRequest.Create(new Uri(url, UriKind.Absolute));
            request.Timeout = -1;
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            BinaryReader reader = new BinaryReader(responseStream);
            MemoryStream memoryStream = new MemoryStream();

            byte[] bytebuffer = new byte[BytesToRead];
            int bytesRead = reader.Read(bytebuffer, 0, BytesToRead);

            while (bytesRead > 0)
            {
                memoryStream.Write(bytebuffer, 0, bytesRead);
                bytesRead = reader.Read(bytebuffer, 0, BytesToRead);
            }

            image.BeginInit();
            memoryStream.Seek(0, SeekOrigin.Begin);

            image.StreamSource = memoryStream;
            image.EndInit();

            return image;
        }

        public static async Task<ImageSource> DownloadImageAsync(string url)
        {
            using var fileRetrievalClient = new HttpClient();

            var image = new BitmapImage();
            var fileResponse = await fileRetrievalClient.GetAsync(url);
            byte[] bytes = await fileResponse.Content.ReadAsByteArrayAsync();

            var memoryStream = new MemoryStream(bytes);
            image.BeginInit();
            memoryStream.Seek(0, SeekOrigin.Begin);

            image.StreamSource = memoryStream;
            image.EndInit();

            return image;
        }

        public static Image ConxpublicvertByteArrayToImage(byte[] inImage)
        {
            var ms = new MemoryStream(inImage);
            Image returnImage = Image.FromStream(ms);
            ms.Dispose();
            return returnImage;
        }

        public static BitmapImage GetImage(byte[] imageAsByteArray)
        {
            try
            {
                using (var ms = new MemoryStream(imageAsByteArray))
                {
                    ms.Position = 0;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = ms;
                    bi.EndInit();
                    bi.Freeze();
                    return bi;
                }
            }
            catch
            {
                try
                {
                    //If it fails the normal way try it again with a convert, possible quality loss.
                    System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();
                    System.Drawing.Image img = (System.Drawing.Image)ic.ConvertFrom(imageAsByteArray);
                    if (img != null)
                    {
                        System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap(img);
                        MemoryStream ms = new MemoryStream();
                        bitmap1.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ms.Position = 0;
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();
                        return bi;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static byte[] ConvertImageToByteArray(Image inImage)
        {
            using (var ms = new MemoryStream())
            {
                inImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static Image ResizeImage(Image _image,Size size, bool overrideHeight, bool overrideWidth, InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic)
        {
            if (_image == null)
                return null;

            int sourceWidth = _image.Width;
            int sourceHeight = _image.Height;

            int destWide = 0;
            int destHigh = 0;
            if (overrideHeight && overrideWidth)
            {
                destWide = size.Width; //ScreenWidth;
                destHigh = size.Height; //ScreenHeight;
            }
            else if (overrideWidth)
            {
                destWide = size.Width;
                destHigh = sourceHeight * destWide / sourceWidth;
            }
            else if (overrideHeight)
            {
                destHigh = size.Height;
                destWide = sourceWidth * destHigh / sourceHeight;
            }

            Bitmap b = new Bitmap(destWide, destHigh);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.InterpolationMode = interpolationMode;
                g.DrawImage(_image, 0, 0, destWide, destHigh);
            }

            _image = b;

            return _image;
            //float nPercent = 0;

            ////if (!overrideHeight && overrideWidth)
            ////{
            ////    size.Width = ScreenWidth;
            ////}

            //float nPercentW = (size.Width / (float)sourceWidth);
            //float nPercentH = (size.Height / (float)sourceHeight);

            //if (!overrideHeight && !overrideWidth)
            //{
            //    if (nPercentH < nPercentW)
            //        nPercent = nPercentH;
            //    else
            //        nPercent = nPercentW;
            //}
            //else if (overrideHeight && !overrideWidth)
            //    nPercent = nPercentH;
            //else if (!overrideHeight && overrideWidth)
            //{
            //    nPercent = nPercentW;
            //}

            //int destWidth = (int)(sourceWidth * nPercent);
            //int destHeight = (int)(sourceHeight * nPercent);

            //if (overrideHeight && overrideWidth)
            //{
            //    destWidth = size.Width; //ScreenWidth;
            //    destHeight = size.Height; //ScreenHeight;
            //}

            //Bitmap b = new Bitmap(destWidth, destHeight);
            //using (Graphics g = Graphics.FromImage(b))
            //{
            //    g.InterpolationMode = interpolationMode;
            //    g.DrawImage(_image, 0, 0, destWidth, destHeight);
            //}

            //_image = b;
        }

    }
}

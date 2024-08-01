using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UtilityTools.Core.Infrastructure
{
    public class StreamToImage
    {
        /// <summary>
        /// Return a memory stream from a BitmapImage
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static MemoryStream GetStreamFromImage(BitmapImage imageSource)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(memStream);
            return memStream;
        }

        /// <summary>
        /// Create a BitmapImage from a memory stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static BitmapImage GetImageFromStreamBug(MemoryStream stream)
        {
            //return GetImageFromStreamBug(stream, 0);
            return GetImage(stream.ToArray());
        }

        /// <summary>
        /// Create a BitmapImage from a stream with the specified width (height ratio kept) if not zero
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static BitmapImage GetImageFromStreamBug(MemoryStream stream, int width)
        {
            MemoryStream stream2 = new MemoryStream();
            stream.WriteTo(stream2);
            stream.Flush();
            stream.Close();
            stream2.Position = 0;

            BitmapImage myImage = new BitmapImage();
            myImage.BeginInit();
            myImage.StreamSource = stream2;
            if (width != 0)
                myImage.DecodePixelWidth = width;
            myImage.EndInit();

            //stream.Close();
            stream = null;
            stream2 = null;

            return myImage;
        }

        private static BitmapImage GetImage(byte[] imageAsByteArray)
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

        /// <summary>
        /// Create a BitmapImage from a stream with the specified width (height ratio kept) if not zero
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="resize"></param>
        /// <returns></returns>
        public static BitmapImage GetImageFromStream(Stream stream, int width)
        {
            BitmapImage myImage = new BitmapImage();
            myImage.BeginInit();
            myImage.StreamSource = stream;
            if (width != 0)
                myImage.DecodePixelWidth = width;
            myImage.EndInit();

            return myImage;
        }
    }
}

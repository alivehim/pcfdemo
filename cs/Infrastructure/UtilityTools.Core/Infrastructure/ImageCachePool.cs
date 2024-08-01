using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Utilites;

namespace UtilityTools.Core.Infrastructure
{
    public static class ImageCachePool
    {
        private static ConcurrentDictionary<string, object> imagesPool = new ConcurrentDictionary<string, object>();
        private static ConcurrentDictionary<string, object> defaultimagesPool = new ConcurrentDictionary<string, object>();
        public static object GetCustomImageSource(string pngName)
        {
            return imagesPool.GetOrAdd(pngName, t =>
            {
                return ImageHelper.GetCustomImageSourceFormName(pngName);
            });
        }


        public static object GetDefaultImageSource(string folderName, string pngName)
        {
            return defaultimagesPool.GetOrAdd(pngName, (png, folder) =>
            {
                return ImageHelper.GetDefaultImageSourceFormName(folder, pngName);
            }, folderName);

        }

    }
}

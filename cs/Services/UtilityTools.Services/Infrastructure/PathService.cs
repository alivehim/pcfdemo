using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Utilites;
using UtilityTools.Services.Interfaces.Infrastructure;

namespace UtilityTools.Services.Infrastructure
{
    public class PathService : IPathService
    {
        private readonly string TEMPORARYDIRECTORY = "TEMP";

        //private readonly string PUBLICDIRECTORY = "public";
        private DirectoryInfo tempLocation;
        //private DirectoryInfo mediaLocation;
        private DirectoryInfo thumbnailLocation;
        private DirectoryInfo bookLocation;
        private DirectoryInfo customPngsLocation;
        private DirectoryInfo defaultPngsLocation;
        private DirectoryInfo keywordLocation;

        ///// <summary>
        ///// 视频存放根目录
        ///// </summary>
        //public DirectoryInfo MediaLocation
        //{
        //    get
        //    {
        //        if (mediaLocation == null)
        //        {
        //            var mediaDirectory = ConfigurationManager.AppSettings["MediaLocation"].ToString();
        //            mediaLocation = new DirectoryInfo(mediaDirectory);
        //            if (!mediaLocation.Exists)
        //            {
        //                mediaLocation.Create();
        //            }

        //            return mediaLocation;
        //        }
        //        return mediaLocation;
        //    }
        //}

        /// <summary>
        /// 临时文件根目录
        /// </summary>
        public DirectoryInfo TemporaryLocation
        {
            get
            {
                if (tempLocation == null)
                {
                    var tmpDirectory = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\{TEMPORARYDIRECTORY}";
                    tempLocation = new DirectoryInfo(tmpDirectory);
                    if (!tempLocation.Exists)
                    {
                        tempLocation.Create();
                    }

                    return tempLocation;
                }
                return tempLocation;
            }
        }

        public DirectoryInfo ThumbnailLocation
        {
            get
            {
                if (thumbnailLocation == null)
                {
                    var thumbnailLocationPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Thumbnail";
                    thumbnailLocation = new DirectoryInfo(thumbnailLocationPath);
                    if (!thumbnailLocation.Exists)
                    {
                        thumbnailLocation.Create();
                    }

                    return thumbnailLocation;
                }
                return thumbnailLocation;
            }
        }


        public DirectoryInfo BookLocation
        {
            get
            {
                if (bookLocation == null)
                {
                    var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Book";
                    bookLocation = new DirectoryInfo(path);
                    if (!bookLocation.Exists)
                    {
                        bookLocation.Create();
                    }

                    return bookLocation;
                }
                return bookLocation;
            }
        }

        public DirectoryInfo CustomPngsLocation
        {
            //get
            //{
            //    if (customPngsLocation == null)
            //    {
            //        var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\CustomPngs";
            //        customPngsLocation = new DirectoryInfo(path);
            //        if (!customPngsLocation.Exists)
            //        {
            //            customPngsLocation.Create();
            //        }

            //        return customPngsLocation;
            //    }
            //    return customPngsLocation;
            //}

            get
            {
                if (customPngsLocation == null)
                {
                    var path = $"{Settings.Current.OneDriveFolder}\\Profiles";
                    customPngsLocation = new DirectoryInfo(path);
                    if (!customPngsLocation.Exists)
                    {
                        customPngsLocation.Create();
                    }

                    return customPngsLocation;
                }
                return customPngsLocation;
            }
        }

        public DirectoryInfo DefaultPngsLocation
        {
            get
            {
                if (defaultPngsLocation == null)
                {
                    var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\DefaultPngs";
                    defaultPngsLocation = new DirectoryInfo(path);
                    if (!defaultPngsLocation.Exists)
                    {
                        defaultPngsLocation.Create();
                    }

                    return defaultPngsLocation;
                }
                return defaultPngsLocation;
            }
        }

        public DirectoryInfo KeywordLocation
        {
            get
            {
                if (keywordLocation == null)
                {
                    var path = $"{Settings.Current.OneDriveFolder}\\KeyFiles";
                    keywordLocation = new DirectoryInfo(path);
                    if (!keywordLocation.Exists)
                    {
                        keywordLocation.Create();
                    }

                    return keywordLocation;
                }
                return keywordLocation;
            }
        }


        public PathService(
           )
        {
        }

        /// <summary>
        /// 清除临时文件夹
        /// </summary>
        /// <returns></returns>
        public bool ClearTemperary()
        {
            var tmpDirectory = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\{TEMPORARYDIRECTORY}";
            var tmp = new DirectoryInfo(tmpDirectory);
            if (tmp.Exists)
            {
                //clear all the  files of directory
                FileHelper.DelectDir(tmp.FullName);
            }
            return true;
        }

        public string GetNugetPath()
        {
            return $"{System.AppDomain.CurrentDomain.BaseDirectory}\\nuget.exe";
        }
    }
}

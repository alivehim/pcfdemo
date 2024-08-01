using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Utilites;
using UtilityTools.Engine.Download;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.Core.Could.Download;
using UtilityTools.Services.Interfaces.Infrastructure;

namespace UtilityTools.Engine
{
    public class ImageDownloadEngine : IImageDownloadEngine
    {
        private readonly IDownloderSelector downloderSelector;
        private readonly IImageCollectorFactory imageCollectorFactory;

        private int errorNum = 0;


        public int ErrorNum
        {
            get
            {
                return errorNum;
            }
        }

        public ImageDownloadEngine(IDownloderSelector downloderSelector, IImageCollectorFactory imageCollectorFactory)
        {
            this.downloderSelector = downloderSelector;
            this.imageCollectorFactory = imageCollectorFactory;
        }

        public Task Run(IImageUXItemDescription item, CancellationToken token)
        {
            var task = Task.Run(async () =>
            {
                try
                {
                    item.IsDownloading = true;
                    await ProcessAsync(item, token);
                }
                catch (Exception ex)
                {
                    item.Error(ex.ToString());
                    item.TaskStage = TaskStage.Error;
                    //DownloadOkHandler(item);
                }
                finally
                {
                    item.IsDownloading = false;
                }
            }, token);

            return task;
        }

        private async Task ProcessAsync(IImageUXItemDescription item, CancellationToken token)
        {

            //如果Media文件已存在
            if (!Directory.Exists(item.StoragePath))
            {
                Directory.CreateDirectory(item.StoragePath);
            }

            string zipPath = Path.Combine(Directory.GetParent(item.StoragePath).FullName, @$".\{item.Name}.zip");

            if (File.Exists(zipPath))
            {
                item.TaskStage = TaskStage.Done;

                return;
            }

            await DownloadImageFilesAsync(item, token);

            if (errorNum <= 3)
            {
                ZipFiles(item);

                item.TaskStage = TaskStage.Done;
            }
            else
            {
                item.TaskStage = TaskStage.Error;
            }
           
        }

        private async Task<IList<MediaDataDescription>> GetDownloadFilesAsync(IImageUXItemDescription description)
        {
            var collector = imageCollectorFactory.GetImageCollector(description.Url);

            return await collector.GetImagesAsync(description.Url);
        }

        private async Task DownloadImageFilesAsync(IImageUXItemDescription description, CancellationToken token)
        {
            var downloader = downloderSelector.GetDownloader();
            description.Info($"select {downloader.Address}");

            try
            {
                var items = await GetDownloadFilesAsync(description);

                description.NumberOfChunkFiles=items.Count;

                foreach (var item in items)
                {
                    var fileName = $"{description.StoragePath}/{item.Order.ToString().PadLeft(3, '0')}.jpg";
                    if (!File.Exists(fileName))
                    {
                        description.Info($" downloading {item.ImageUrl}");
                        //if( downloader.DownloadFile(item.ImageUrl, fileName))
                        // {

                        // }

                        if (!DowloadThread.RunTry(() =>
                        {
                            if (!downloader.DownloadFile(item.ImageUrl, fileName))
                            {
                                throw new Exception("error download");
                            }
                            return true;
                        }, item, (item, ex, count) =>
                        {
                            description.Info($"执行失败! 重试次数 {count}-{ex.ToString()}");
                            //异常，删除文件
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }

                        }))
                        {
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }
                            description.Error($"下载文件{item.ImageUrl}失败");

                            Interlocked.Increment(ref errorNum);

                        }
                    }

                    description.CompletedNumber++;
                    decimal percentDone = ((decimal)((decimal)description.CompletedNumber / (decimal)description.NumberOfChunkFiles) * (decimal)100);

                    description.Progress = (int)percentDone;

                }
            }
            catch (Exception ex)
            {
                description.Error(ex.Message);

                errorNum += 100;
            }
            finally
            {
                description.Info($"return downloader");
                downloderSelector.Return(downloader);
            }


        }

        private void ZipFiles(IImageUXItemDescription description)
        {
            string zipPath = Path.Combine(Directory.GetParent(description.StoragePath).FullName, @$".\{description.Name}.zip");

            ZipFile.CreateFromDirectory(description.StoragePath, zipPath);

            //delete folder

            FileHelper.DelectDir(description.StoragePath);

            description.StoragePath = zipPath;
        }
    }
}

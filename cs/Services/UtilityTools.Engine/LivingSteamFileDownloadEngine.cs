using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Infrastructure.FFMPEG;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Utilites;
using UtilityTools.Engine.Download;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.Core.Could.ChunkFile;
using UtilityTools.Services.Interfaces.Core.Could.Download;
using UtilityTools.Services.Interfaces.Infrastructure;

namespace UtilityTools.Engine
{
    public class LivingSteamFileDownloadEngine : ILivingStreamMediaDownloadEngine
    {

        private readonly IChunkFileProvider chunkFileProvider;
        //private readonly IChunkFileDownloader chunkFileDownloader;
        private readonly IChunkFileCombiner chunkFileCombiner;
        private readonly IPathService pathService;
        private readonly IDownloadProvider downloadProvider;

        private readonly IDefaultChunkFileDownloader defaultChunkFileDownloader;

        public LivingSteamFileDownloadEngine(IChunkFileProvider chunkFileProvider, IChunkFileCombiner chunkFileCombiner, IPathService pathService, IDownloadProvider downloadProvider, IDefaultChunkFileDownloader defaultChunkFileDownloader)
        {
            this.chunkFileProvider = chunkFileProvider;
            this.chunkFileCombiner = chunkFileCombiner;
            this.pathService = pathService;
            this.downloadProvider = downloadProvider;
            this.defaultChunkFileDownloader = defaultChunkFileDownloader;
        }

        //private readonly IEventAggregator eventAggregator;


        public  Task Run(IStreamUXItemDescription item)
        {

            return Task.Run(async () =>
            {
                try
                {

                    await ProcessAsync(item);
                }
                catch (Exception ex)
                {
                    item.Error(ex.ToString());
                    item.TaskStage = TaskStage.Error;

                    //DownloadOkHandler(item);
                }
                finally
                {
                    item.IsWaiting = false;
                    item.IsDownloading = false;
                }
            });
           
        }

        private void DownloadOkHandler(IBaseUXItemDescription item)
        {
            item.TaskStage = TaskStage.Done;
            //var itemFinishedEvent = this.eventAggregator.GetEvent<MediaDownloadCompletedEvent>();
            //itemFinishedEvent.Publish(item);
        }


        private async Task ProcessAsync(IStreamUXItemDescription item)
        {
            //准备临时文件夹
            PrepareTempFile(item);

            //如果Media文件已存在
            if (!File.Exists(item.StoragePath))
            {
                //如果ts文件已存在
                if (!File.Exists(item.CombinedFile))
                {
                    item.Progress = 0;

                    //Get Chunk  file list
                    await GetChunkFileListAsync(item, item.CancellationTokenSource.Token);
                    DownloadChunkFiles(item, item.CancellationTokenSource.Token);
                }
                else
                {
                    item.Progress = 100;
                }

                if (!item.IsCancelPending && !item.CancellationTokenSource.Token.IsCancellationRequested)
                {
                    ConvertFile(item, item.CancellationTokenSource.Token);
                }
                else
                {
                    item.TaskStage = TaskStage.Done;
                }
            }
            else
            {
                //item.DownloadUrl = item.MediaFile;
                item.TaskStage = TaskStage.Done;
                DownloadOkHandler(item);
            }


        }

        private string GetPath(IStreamUXItemDescription item)
        {
            return item.Name;
        }

        /// <summary>
        /// 准备临时文件夹
        /// </summary>
        /// <param name="item"></param>
        private void PrepareTempFile(IStreamUXItemDescription item)
        {
            item.Info("prepare temp file ");
            var TempDirectory = $"{pathService.TemporaryLocation}\\{GetPath(item)}";
            DirectoryInfo chunkDirectory = null;
            if (!Directory.Exists(TempDirectory))
            {
                var info = Directory.CreateDirectory(TempDirectory);
                chunkDirectory = info.CreateSubdirectory("chunk");
            }
            else
            {
                chunkDirectory = new DirectoryInfo(TempDirectory).GetDirectories().FirstOrDefault(p => p.Name == "chunk");
            }


            item.TempDirectory = chunkDirectory?.FullName;
            item.CombinedFile = $"{pathService.TemporaryLocation}\\{item.Name}\\{item.Name}.ts";

        }

        private void DeleteTempFile(IStreamUXItemDescription item)
        {
            try
            {
                item.Info("prepare temp file ");
                var TempDirectory = $"{pathService.TemporaryLocation}\\{GetPath(item)}";

                if (Directory.Exists(TempDirectory))
                {
                    FileHelper.DelectDir(TempDirectory);
                }

            }
            catch (Exception ex)
            {
                item.Info(ex.ToString());
            }
        }

        private async Task GetChunkFileListAsync(IStreamUXItemDescription item, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            item.Info("Start Getting Chunk File List");
            Queue<string> chunkFiles = await chunkFileProvider.GetChunkFiles(item, token);

            item.ParsedChunks = chunkFiles;
            item.NumberOfChunkFiles = item.ParsedChunks.Count;
        }

        private void DownloadChunkFiles(IStreamUXItemDescription item, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            item.Info("Start Downloading Chunk Files");

            if (item.IsNeedDecode)
            {
                var TempDirectory = $"{pathService.TemporaryLocation}\\{GetPath(item)}";
                DirectoryInfo chunkDirectory = null;
                if (!Directory.Exists(TempDirectory))
                {
                    var info = Directory.CreateDirectory(TempDirectory);
                    chunkDirectory = info.CreateSubdirectory("chunk");
                }
                else
                {
                    chunkDirectory = new DirectoryInfo(TempDirectory).GetDirectories().FirstOrDefault(p => p.Name == "chunk");
                }


                item.TempDirectory = chunkDirectory?.FullName;
            }


            defaultChunkFileDownloader.DownloadFileChunks(item, token);
            if (defaultChunkFileDownloader.ErrorNum > 10)
            {
                defaultChunkFileDownloader.DownloadFileChunks(item, token);
            }

        }

        //private void CombineChunkFiles(IStreamUXItemDescription item, CancellationToken token)
        //{

        //    if (token.IsCancellationRequested)
        //        return;

        //    if (!item.IsNeedDecode)
        //    {
        //        item.Info("Start Combining Chunk Files");
        //        var _combineChunksCancellationTokenSource = new CancellationTokenSource();
        //        var _combineChunksCancellationToken = _combineChunksCancellationTokenSource.Token;

        //        IEnumerable<FileInfo> chunkFiles = chunkFileCombiner.GetChunkFileInfos(item.TempDirectory);

        //        chunkFileCombiner.CombineChunkFiles(
        //            item,
        //            chunkFiles,
        //            item.CombinedFile,
        //            _combineChunksCancellationToken);
        //    }
        //}

        public void ConvertFile(IStreamUXItemDescription item, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            item.Info($"Convert File {item.CombinedFile}");
            FrapperWrapper frapperWrapper = new FrapperWrapper(new FFMPEG());


            item.Info($@" -allowed_extensions ALL  -protocol_whitelist ""file,http,https,rtp,udp,tcp,tls,crypto"" -i ""{Settings.Current.M3u8LocationRoot}\{item.Name}\list.m3u8"" -c copy -bsf:a aac_adtstoasc ""{item.StoragePath}""");
            frapperWrapper.ExecuteMultipleCommand($@"-allowed_extensions ALL -protocol_whitelist ""file,http,https,rtp,udp,tcp,tls,crypto"" -i ""{Settings.Current.M3u8LocationRoot}\{item.Name}\list.m3u8"" -c copy -bsf:a aac_adtstoasc ""{item.StoragePath}""",
                (obj, e) =>
                {
                    item.Info($"Receive {e.Data}");
                });


            item.TaskStage = TaskStage.Done;

            if (Settings.Current.DeleteTempFolder)
            {
                if (File.Exists(item.StoragePath))
                {
                    //删除临时文件
                    DeleteTempFile(item);
                }

            }

            try
            {

                if (File.Exists(item.CombinedFile))
                    File.Delete(item.CombinedFile);
            }
            catch
            { }

            DownloadOkHandler(item);
        }
    }
}

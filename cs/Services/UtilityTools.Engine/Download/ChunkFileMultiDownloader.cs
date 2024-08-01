using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using UtilityTools.Core.Extensions;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.Core.Could.ChunkFile;
using UtilityTools.Services.Interfaces.Core.Could.Download;

namespace UtilityTools.Engine.Download
{
    public class VLCDownloadItem
    {
        public int Order { get; set; }
        public string ResourceUri { get; set; }
    }

    public class ChunkFileMultiDownloader : IDefaultChunkFileDownloader, IDisposable
    {
        private ConcurrentQueue<VLCDownloadItem> _queue = new ConcurrentQueue<VLCDownloadItem>();
        private const string _outputStreamFileExtension = ".ts";

        private Task[] downloadTasks = new Task[5];

        //private IRPCDownloader[] downloader;
        //private IRPCDownloader[] anotherDownloader;

        private string tempDirectory = string.Empty;
        private IStreamUXItemDescription item;
        private int totalDownloadNum = 0;
        private static object obj = new object();
        private int comletedNum = 0;
        private int errorNum = 0;

        private readonly IDownloderSelector downloderSelector;

        private System.Timers.Timer timer = new System.Timers.Timer(5 * 1000);

        public int ErrorNum
        {
            get
            {
                return errorNum;
            }
        }


        public ChunkFileMultiDownloader(IDownloderSelector downloderSelector )
        {
          
            this.downloderSelector = downloderSelector;


            timer.Enabled = false;
        }

        

        public void DownloadFileChunks(IStreamUXItemDescription item,CancellationToken cancellationToken)
        {
            this.item = item;
            this.tempDirectory = item.TempDirectory;
            comletedNum = 0;
            errorNum = 0;
            item.Progress = 0;
            Directory.CreateDirectory(tempDirectory);

            totalDownloadNum = item.ParsedChunks.Count;

            for (int uriIteration = 0; uriIteration < totalDownloadNum; uriIteration++)
            {
                _queue.Enqueue(new VLCDownloadItem { Order = uriIteration, ResourceUri = item.ParsedChunks.Dequeue() });
            }

            //if (!item.IsDownloadSpecial)
            //{
            //    for (int i = 0; i < downloadTasks.Length; i++)
            //    {
            //        //决策
            //        var idownloader = !item.IsDownloadSpecial ? downloader[i] : anotherDownloader[i];

            //        downloadTasks[i] = Task.Run(() =>
            //        {
            //            Download(idownloader, cancellationToken);
            //        });
            //    }
            //}
            //else
            {
                downloadTasks = new Task[5];
                int baseNum = item.DownloadTag * 5;
                for (int i = 0; i < downloadTasks.Length; i++)
                {

                    downloadTasks[i] = Task.Run(() =>
                    {
                        Download(item, cancellationToken);
                    });
                }
            }

            if (!timer.Enabled)
            {
                timer.Elapsed += OnTimedEvent;
                timer.Enabled = true;
            }

            Task.WaitAll(downloadTasks, cancellationToken);
            item.Progress = 100;

            timer.Elapsed -= OnTimedEvent;
            timer.Enabled = false;
        }

        private void Download(IStreamUXItemDescription item,  string currentUri, string filePath)
        {
            var downloader = downloderSelector.GetDownloader();
            try
            {
               
                // 文件存在则不下载
                if (!File.Exists(filePath))
                {
                    if (!DowloadThread.RunTry(() =>
                     {
                         if (!item.IsCancelPending)
                         {
                          
                             //item.Info($"select {downloader.Address}");
                             if (!downloader.DownloadFile(currentUri, filePath))
                             {
                                 throw new Exception(" download failed ");
                             }
                         }
                       
                         return true;
                     }, item, (item, ex, count) =>
                     {
                         item.Info($"执行失败! 重试次数 {count}-{ex.ToString()}");
                         //异常，删除文件
                         if (File.Exists(filePath))
                         {
                             File.Delete(filePath);
                         }

                     }))
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        item.Error($"下载文件{currentUri}失败");

                        Interlocked.Increment(ref errorNum);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught");
                item.Error(ex.ToString());
            }
            finally
            {
                downloderSelector.Return(downloader);
                Interlocked.Increment(ref comletedNum);
            }
        }

        private void Download(IStreamUXItemDescription item, CancellationToken cancellationToken)
        {
            while (_queue.Count > 0 && (!cancellationToken.IsCancellationRequested && !item.IsCancelPending))
            {
                if (_queue.TryDequeue(out VLCDownloadItem resource))
                {

                    string filePath = GetIndividualChunkFileName(
                    tempDirectory,
                    resource.Order,
                    totalDownloadNum.ToString().Length
                    );

                    Download(item, resource.ResourceUri, filePath);
                }
                else
                    break;
            }
        }

        private string GetIndividualChunkFileName(string tempDirectory, int uniqueIteration, int requiredPadding)
        {
            StringBuilder streamingFilePathBuilder = new StringBuilder();
            streamingFilePathBuilder.Clear();
            streamingFilePathBuilder.Append("chunk");
            streamingFilePathBuilder.Append('_');
            streamingFilePathBuilder.Append(uniqueIteration.ToString().PadLeft(requiredPadding, '0'));
            streamingFilePathBuilder.Append(_outputStreamFileExtension);

            return Path.Combine(
                tempDirectory,
                streamingFilePathBuilder.ToString()
                );
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            decimal percentDone = ((decimal)((decimal)comletedNum / (decimal)totalDownloadNum) * (decimal)100);

            item.CompletedNumber = comletedNum;

            item.Progress = (int)percentDone;
        }

        public void Dispose()
        {
            this.timer.Enabled = false;
            timer.Dispose();
        }

    }
}

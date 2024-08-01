using Polly;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UtilityTools.Core.Extensions;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could.ChunkFile;
using UtilityTools.Services.Interfaces.Core.Could.Download;

namespace UtilityTools.Engine.ChunkFile
{
    public class ChunkFileDownloader : IChunkFileDownloader, IDisposable
    {
        private const string _outputStreamFileExtension = ".ts";
        private readonly WebClient _webClient;
        private IRPCDownloader _rpcDownloader;

        public ChunkFileDownloader(IRPCDownloader rpcDownloader)
        {
            _webClient = new WebClient();
            _rpcDownloader = rpcDownloader;
        }



        public void DownloadFileChunks(IStreamUXItemDescription item, string tempDirectory, CancellationToken cancellationToken)
        {

            item.Progress = 0;

            Directory.CreateDirectory(tempDirectory);

            int numberOfUris = item.ParsedChunks.Count;

            StringBuilder streamingFilePathBuilder = new StringBuilder();

            for (int uriIteration = 0; uriIteration < numberOfUris; uriIteration++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var currentUri = item.ParsedChunks.Dequeue();

                string filePath = GetIndividualChunkFileName(
                    tempDirectory,
                    uriIteration,
                    numberOfUris.ToString().Length,
                    streamingFilePathBuilder
                    );

                try
                {
                    //byte[] data = _webClient.DownloadData(currentUri);
                    //File.WriteAllBytes(filePath, data);

                    var retryTwoTimesPolicy =
                    Policy
                        .Handle<TimeoutException>()
                        .Retry(3, (ex, count) =>
                        {
                            item.Error($"执行失败! 重试次数 {count}");
                            item.Error($"异常来自 {ex.GetType().Name}");
                        });
                    retryTwoTimesPolicy.Execute(() =>
                    {
                        //Download(currentUri, filePath);
                        _rpcDownloader.DownloadFile(currentUri.ToString(), filePath);
                    });

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught");
                    item.Error(ex.ToString());
                }
                finally
                {
                    decimal percentDone = ((decimal)((decimal)uriIteration / (decimal)numberOfUris) * (decimal)100);

                    item.Progress = Convert.ToInt32(percentDone);
                    item.CompletedNumber += 1;

                    item.Info(string.Format("{0} Downloaded: {1}", uriIteration, filePath));
                }
            }

            item.Progress = 100;
        }

        private void Download(Uri currentUri, string filePath)
        {
            byte[] data = _webClient.DownloadData(currentUri);
            File.WriteAllBytes(filePath, data);
        }

        private string GetIndividualChunkFileName(string tempDirectory, int uniqueIteration, int requiredPadding, StringBuilder streamingFilePathBuilder)
        {
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

        ~ChunkFileDownloader()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {
                // 释放 托管资源 
                if (_webClient != null)
                {
                    _webClient.Dispose();
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

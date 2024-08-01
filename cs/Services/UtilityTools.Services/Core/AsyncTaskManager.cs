using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.Engine;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.Queue;

namespace UtilityTools.Services.Core
{
    public class AsyncTaskManager : IAsyncTaskManager
    {
        private CancellationToken stoppingToken => tokenSource.Token;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        private IList<Task> tasks = new
            List<Task>();

        //public ConcurrentDictionary<IStreamUXItemDescription, Task> Container = new ConcurrentDictionary<IStreamUXItemDescription, Task>();

        private readonly ISimpleBackgroundTaskQueue<IStreamUXItemDescription> taskQueue;
        private readonly ILivingMediaDownloadEngineFactory livingMediaDownloadEngineFactory;
        private readonly IStreamFileDownloadEngine streamFileDownloadEngine ;

        public AsyncTaskManager(ISimpleBackgroundTaskQueue<IStreamUXItemDescription> taskQueue, ILivingMediaDownloadEngineFactory livingMediaDownloadEngineFactory, IStreamFileDownloadEngine streamFileDownloadEngine)
        {
            this.taskQueue = taskQueue;
            this.livingMediaDownloadEngineFactory = livingMediaDownloadEngineFactory;
            this.streamFileDownloadEngine = streamFileDownloadEngine;
        }

        public bool IsRunning { get; private set; }

        public void QueueItem(IStreamUXItemDescription streamUXItemDescription)
        {
            streamUXItemDescription.IsWaiting = false;
            taskQueue.QueueBackgroundWorkItem(streamUXItemDescription);
        }

        public void Remove(IStreamUXItemDescription streamUXItemDescription)
        {
            taskQueue.Remove(streamUXItemDescription);
        }

        
        private async Task TaskBody()
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await taskQueue.DequeueAsync(stoppingToken);

                try
                {

                    if (workItem != null)
                    {
                        workItem.IsDownloading = true;

                        if(workItem.IsLivingStream)
                        {
                            var engine = livingMediaDownloadEngineFactory.GetHandler((LivingStreamDownloadProvider)Settings.Current.DownloadProvider);
                            workItem.TaskStage = TaskStage.Doing;

                            await engine.Run(workItem);
                        }
                        else
                        {
                           await streamFileDownloadEngine.Run(workItem);
                        }
                      

                    }
                }
                catch
                {

                }
            }
        }

        public void Start()
        {
            IsRunning = true;
            Task.Run(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    tasks.Add(Task.Run(async () => {
                        await TaskBody();
                    }));
                }

                Task.WaitAll(tasks.ToArray());
            });
          
        }

        public void Stop()
        {
            tokenSource.Cancel();
        }

 

    }
}

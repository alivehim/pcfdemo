using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Infrastructure.Queue;

namespace UtilityTools.Services.Infrastructure.Queue
{
    public class TaskQueueResolver<T> : ITaskQueueResolver<T>
    {
        private CancellationToken stoppingToken;
        public IBackgroundTaskQueue<T> TaskQueue { get; }

        private bool isrunning = false;

        //private Action<IStreamUXItemDescription> func;
        private Action<T> func;
        private Task task;

        //private Func<T, Task> delayFunc;

        public TaskQueueResolver(IBackgroundTaskQueue<T> taskQueue)
        {
            TaskQueue = taskQueue;
        }

        public void Start(CancellationToken token)
        {
            stoppingToken = token;

            if (!isrunning)
            {
                isrunning = true;
                task = Task.Run(async () =>
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var workItem = await TaskQueue.DequeueAsync(stoppingToken);
                        var item = await workItem(stoppingToken);

                        try
                        {

                            //await delayFunc(item);
                            func?.Invoke(item);
                        }
                        catch
                        {

                        }
                    }
                });
            }

        }

        public void Stop()
        {

        }

        public void RegisterAction(Action<T> func)
        {
            this.func = func;
        }

        //public void RegisterAction(Func<T, Task> func)
        //{
        //    this.delayFunc = func;
        //}
    }
}

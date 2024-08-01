using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure.Queue
{
    public interface ITaskQueueResolver<T>
    {
        //IBackgroundTaskQueue<IStreamUXItemDescription> TaskQueue { get; }
        IBackgroundTaskQueue<T> TaskQueue { get; }

        //void RegisterAction(Action<T> func);
        void RegisterAction(Action<T> func);

        //void RegisterAction(Func<T, Task> func);
        void Start(CancellationToken token);
        void Stop();
    }

    public interface ITaskQueueResolverAsync<T>
    {
        //IBackgroundTaskQueue<IStreamUXItemDescription> TaskQueue { get; }
        IBackgroundTaskQueue<T> TaskQueue { get; }


        void RegisterAction(Func<T, Task> func);
        void Start(CancellationToken token);
        void Stop();
    }
}

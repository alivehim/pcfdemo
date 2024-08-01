using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Infrastructure.Queue;

namespace UtilityTools.Services.Infrastructure.Queue
{
    public class BackgroundTaskQueue<T> : IBackgroundTaskQueue<T>
    {
        private ConcurrentQueue<Func<CancellationToken, Task<T>>> _workItems =
            new ConcurrentQueue<Func<CancellationToken, Task<T>>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        /// <summary>
        /// 进入队列
        /// </summary>
        /// <param name="workItem"></param>
        public void QueueBackgroundWorkItem(
            Func<CancellationToken, Task<T>> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

  
        /// <summary>
        /// 出队列
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Func<CancellationToken, Task<T>>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }

    }
}

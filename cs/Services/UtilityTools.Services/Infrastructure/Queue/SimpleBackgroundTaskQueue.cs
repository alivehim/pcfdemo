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
    public class SimpleBackgroundTaskQueue<T> : ISimpleBackgroundTaskQueue<T> where T : class
    {
        private ConcurrentQueue<T> _workItems =
            new ConcurrentQueue<T>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        /// <summary>
        /// 进入队列
        /// </summary>
        /// <param name="workItem"></param>
        public void QueueBackgroundWorkItem(
            T workItem)
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
        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }

        public void Remove(T workItem)
        {
            if (_workItems.Contains(workItem) == true)
            {
                _workItems = new ConcurrentQueue<T>(_workItems.Where(p => p != workItem));
                return;
            }
        }

    }
}

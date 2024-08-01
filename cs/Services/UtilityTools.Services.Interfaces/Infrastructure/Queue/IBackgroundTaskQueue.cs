using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure.Queue
{
    public interface IBackgroundTaskQueue<T>
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task<T>> workItem);

        Task<Func<CancellationToken, Task<T>>> DequeueAsync(
            CancellationToken cancellationToken);
    }
}

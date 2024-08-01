using System.Threading;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure.Queue
{
    public interface ISimpleBackgroundTaskQueue<T>
    {
        Task<T> DequeueAsync(CancellationToken cancellationToken);
        void QueueBackgroundWorkItem(T workItem);
        void Remove(T workItem);
    }
}
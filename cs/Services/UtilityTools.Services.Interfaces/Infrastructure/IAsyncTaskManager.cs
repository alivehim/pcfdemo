using UtilityTools.Services.Interfaces;

namespace UtilityTools.Services.Interfaces.Infrastructure
{
    public interface IAsyncTaskManager
    {
        bool IsRunning { get; }

        void QueueItem(IStreamUXItemDescription streamUXItemDescription);
        void Remove(IStreamUXItemDescription streamUXItemDescription);
        void Start();
        void Stop();
    }
}
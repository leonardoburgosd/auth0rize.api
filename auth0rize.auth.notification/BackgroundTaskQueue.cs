using auth0rize.auth.domain.Primitives;
using System.Collections.Concurrent;

namespace auth0rize.auth.notification
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, ValueTask>> _workItems =
            new ConcurrentQueue<Func<CancellationToken, ValueTask>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem)
        {
            if (workItem is null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }
}

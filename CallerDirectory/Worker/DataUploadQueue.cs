using CallerDirectory.Models;
using System.Collections.Concurrent;

namespace CallerDirectory.Worker
{
    public class DataUploadQueue : IDataUploadQueue
    {
        private readonly ConcurrentQueue<CallRecord> queue = new();

        public bool IsEmpty { get => this.queue.IsEmpty; }


        public void Enqueue(CallRecord record) => this.queue.Enqueue(record);

        public bool TryDequeue(out CallRecord? record) => this.queue.TryDequeue(out record);
    }
}

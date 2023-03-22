using CallerDirectory.Models;

namespace CallerDirectory.Worker
{
    public interface IDataUploadQueue
    {
        public bool IsEmpty { get; }

        public void Enqueue(CallRecord record);

        public bool TryDequeue(out CallRecord? record);
    }
}

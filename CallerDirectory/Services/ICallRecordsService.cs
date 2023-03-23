using CallerDirectory.Models;

namespace CallerDirectory.Services
{
    public interface ICallRecordsService
    {
        public Task<CallRecord?> GetRecordAsync(string reference);

        public Task<IEnumerable<CallRecord>> GetRecordsAsync(PaginatedRequest pagination);

        public Task<IEnumerable<CallRecord>> GetCallerRecordsAsync(PaginatedRequest pagination, long? callerId = null);

        public Task<IEnumerable<CallRecord>> GetRecipientRecordsAsync(PaginatedRequest pagination, long recipientId);

        public Task<IEnumerable<object>> GetHourlyCostsAsync();
    }
}

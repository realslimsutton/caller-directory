using CallerDirectory.Models;

namespace CallerDirectory.Services
{
    public interface ICallRecordsService
    {
        public Task<CallRecord?> GetRecordAsync(string reference);

        public Task<IEnumerable<CallRecord>> GetRecordsAsync(PaginatedRequest pagination);
    }
}

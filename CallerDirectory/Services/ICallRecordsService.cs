using CallerDirectory.Models;

namespace CallerDirectory.Services
{
    public interface ICallRecordsService
    {
        public Task<CallRecord?> GetRecordAsync(string reference);

        public Task<IEnumerable<CallRecord>> GetRecordsAsync(Request request);

        public Task<IEnumerable<CallRecord>> GetCallerRecordsAsync(Request request, long? callerId = null);

        public Task<IEnumerable<CallRecord>> GetRecipientRecordsAsync(Request request, long recipientId);

        public Task<IEnumerable<object>> GetCallersCostAsync(Request request);

        public Task<IEnumerable<object>> GetHourlyCostsAsync();
    }
}

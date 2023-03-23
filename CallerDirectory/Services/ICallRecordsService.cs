using CallerDirectory.Models;

namespace CallerDirectory.Services
{
    public interface ICallRecordsService
    {
        public Task<CallRecord?> Get(string reference);
    }
}

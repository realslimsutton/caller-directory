using CallerDirectory.DataAccess;
using CallerDirectory.Models;
using Microsoft.EntityFrameworkCore;

namespace CallerDirectory.Services
{
    public class CallRecordsService : ICallRecordsService
    {
        private readonly IConfiguration _configuration;

        public CallRecordsService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<CallRecord?> Get(string reference)
        {
            using (CallingContext context = this.CreateContext())
            {
                return await context.CallRecords.FirstOrDefaultAsync(c => c.Reference == reference);
            }
        }

        private CallingContext CreateContext()
        {
            return new CallingContext(this._configuration);
        }
    }
}

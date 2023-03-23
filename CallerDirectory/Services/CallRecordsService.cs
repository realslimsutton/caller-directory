using CallerDirectory.DataAccess;
using CallerDirectory.Extensions;
using CallerDirectory.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CallerDirectory.Services
{
    public class CallRecordsService : ICallRecordsService
    {
        private readonly IConfiguration _configuration;

        public CallRecordsService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<CallRecord?> GetRecordAsync(string reference)
        {
            using CallingContext context = this.CreateContext();

            return await context.CallRecords.FirstOrDefaultAsync(c => c.Reference == reference);
        }

        public async Task<IEnumerable<CallRecord>> GetRecordsAsync(Request request)
        {
            using CallingContext context = this.CreateContext();

            return await this.ApplyRequestFiltersToQuery(context.CallRecords.AsQueryable(), request).ToListAsync();
        }

        public async Task<IEnumerable<CallRecord>> GetCallerRecordsAsync(Request request, long? callerId = null)
        {
            using CallingContext context = this.CreateContext();

            return await this.ApplyRequestFiltersToQuery(context.CallRecords.Where(c => c.Caller == callerId), request).ToListAsync();
        }

        public async Task<IEnumerable<CallRecord>> GetRecipientRecordsAsync(Request request, long recipientId)
        {
            using CallingContext context = this.CreateContext();

            return await this.ApplyRequestFiltersToQuery(context.CallRecords.Where(c => c.Recipient == recipientId), request).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetCallersCostAsync(Request request)
        {
            using CallingContext context = this.CreateContext();

            IQueryable<object> query = context.CallRecords
                .GroupBy(c => c.Caller)
                .Select(c => new
                {
                    c.Key,
                    AverageCost = c.Average(a => a.Cost),
                    TotalCost = c.Sum(a => a.Cost)
                });

            var te = context.CallRecords
                .GroupBy(c => c.Caller)
                .Select(c => new
                {
                    c.Key,
                    AverageCost = c.Average(a => a.Cost),
                    TotalCost = c.Sum(a => a.Cost)
                })
                .ToQueryString();

            return await this.ApplyRequestFiltersToQuery(query, request).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetHourlyCostsAsync()
        {
            using CallingContext context = this.CreateContext();

            return await context.CallRecords
                .GroupBy(c => c.StartDateTime.Hour)
                .Select(c => new
                {
                    c.Key,
                    AverageCost = c.Average(a => a.Cost),
                    TotalCost = c.Sum(a => a.Cost)
                })
                .OrderBy(c => c.Key)
                .ToListAsync();
        }

        private CallingContext CreateContext()
        {
            return new CallingContext(this._configuration);
        }

        private IQueryable<T> ApplyRequestFiltersToQuery<T>(IQueryable<T> query, Request request)
        {
            return query.ApplyRequestFilters(request)
                .ApplyRequestSorting(request)
                .ApplyRequestPagination(request);
        }
    }
}

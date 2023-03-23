using CallerDirectory.DataAccess;
using CallerDirectory.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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

        public async Task<IEnumerable<CallRecord>> GetRecordsAsync(PaginatedRequest pagination)
        {
            using CallingContext context = this.CreateContext();

            return await this.CreatePaginatedQuery(context.CallRecords.AsQueryable(), pagination).ToListAsync();
        }

        public async Task<IEnumerable<CallRecord>> GetCallerRecordsAsync(PaginatedRequest pagination, long? callerId = null)
        {
            using CallingContext context = this.CreateContext();

            return await this.CreatePaginatedQuery(context.CallRecords.Where(c => c.Caller == callerId), pagination).ToListAsync();
        }

        public async Task<IEnumerable<CallRecord>> GetRecipientRecordsAsync(PaginatedRequest pagination, long recipientId)
        {
            using CallingContext context = this.CreateContext();

            return await this.CreatePaginatedQuery(context.CallRecords.Where(c => c.Recipient == recipientId), pagination).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetCallersCostAsync(PaginatedRequest pagination)
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

            return await this.CreatePaginatedQuery(query, pagination).ToListAsync();
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

        private IQueryable<T> CreatePaginatedQuery<T>(IQueryable<T> query, PaginatedRequest pagination)
        {
            this.ApplyFilters(ref query, pagination);
            this.ApplySorting(ref query, pagination);

            query = query.Skip(pagination.GetSkip()).Take(pagination.PerPage);

            return query;
        }

        private void ApplySorting<T>(ref IQueryable<T> query, PaginatedRequest pagination)
        {
            if (string.IsNullOrWhiteSpace(pagination.SortColumn))
            {
                return;
            }

            query = query.OrderBy($"{pagination.SortColumn} {pagination.SortDirection}");
        }

        private void ApplyFilters<T>(ref IQueryable<T> query, PaginatedRequest pagination)
        {
            if (pagination.StartDateTime != null)
            {
                query = query.Where("StartDateTime >= @0", pagination.StartDateTime);
            }

            if (pagination.EndDateTime != null)
            {
                query = query.Where("StartDateTime <= @0", pagination.EndDateTime);
            }
        }
    }
}

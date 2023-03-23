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

        public async Task<CallRecord?> GetRecordAsync(string reference)
        {
            using CallingContext context = this.CreateContext();

            return await context.CallRecords.FirstOrDefaultAsync(c => c.Reference == reference);
        }

        public async Task<IEnumerable<CallRecord>> GetRecordsAsync(PaginatedRequest pagination)
        {
            using CallingContext context = this.CreateContext();

            return await this.CreatePaginatedQuery(context, pagination).ToListAsync();
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

        private IQueryable<CallRecord> CreatePaginatedQuery(CallingContext context, PaginatedRequest pagination)
        {
            IQueryable<CallRecord> query = context.CallRecords.AsQueryable();

            this.ApplySorting(ref query, pagination);

            query = query.Skip(pagination.GetSkip()).Take(pagination.PerPage);

            return query;
        }

        private void ApplySorting(ref IQueryable<CallRecord> query, PaginatedRequest pagination)
        {
            if (string.IsNullOrWhiteSpace(pagination.SortColumn))
            {
                return;
            }

            if (pagination.SortDirection.ToUpper() == "DESC")
            {
                query = query.OrderByDescending(c => EF.Property<object>(c, pagination.SortColumn));
            }
            else
            {
                query = query.OrderBy(c => EF.Property<object>(c, pagination.SortColumn));
            }
        }
    }
}

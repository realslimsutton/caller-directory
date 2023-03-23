using CallerDirectory.Models;
using System.Linq.Dynamic.Core;

namespace CallerDirectory.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyRequestFilters<T>(this IQueryable<T> query, Request request)
        {
            if (request.StartDateTime != null)
            {
                query = query.Where("StartDateTime >= @0", request.StartDateTime);
            }

            if (request.EndDateTime != null)
            {
                query = query.Where("StartDateTime <= @0", request.EndDateTime);
            }

            return query;
        }

        public static IQueryable<T> ApplyRequestSorting<T>(this IQueryable<T> query, Request request)
        {
            if (string.IsNullOrWhiteSpace(request.SortColumn))
            {
                return query;
            }

            return query.OrderBy($"{request.SortColumn} {request.SortDirection}");
        }

        public static IQueryable<T> ApplyRequestPagination<T>(this IQueryable<T> query, Request request)
        {
            return query.Skip(request.GetSkip()).Take(request.PerPage);
        }
    }
}

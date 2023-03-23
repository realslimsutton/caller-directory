namespace CallerDirectory.Models
{
    public class PaginatedResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; private set; }

        public PaginatedRequest Pagination { get; private set; }

        public PaginatedResponse(IEnumerable<T> data, PaginatedRequest pagination)
        {
            this.Data = data;
            this.Pagination = pagination;
        }
    }
}

namespace CallerDirectory.Models
{
    public class PaginatedResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; private set; }

        public Request Request { get; private set; }

        public PaginatedResponse(IEnumerable<T> data, Request request)
        {
            this.Data = data;
            this.Request = request;
        }
    }
}

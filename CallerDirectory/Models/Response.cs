namespace CallerDirectory.Models
{
    public class Response<T>
    {
        public T Data { get; set; }

        public Response(T data)
        {
            this.Data = data;
        }
    }
}

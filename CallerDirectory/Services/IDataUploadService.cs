namespace CallerDirectory.Services
{
    public interface IDataUploadService
    {
        public Task ImportAsync(Stream stream);
    }
}

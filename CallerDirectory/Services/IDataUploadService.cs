namespace CallerDirectory.Services
{
    public interface IDataUploadService
    {
        public Task Import(Stream stream);
    }
}

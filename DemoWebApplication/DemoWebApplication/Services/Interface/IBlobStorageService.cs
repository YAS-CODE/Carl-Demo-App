namespace DemoWebApplication.Services.Interface
{
    public interface IBlobStorageService
    {
        public Task<List<string>> GetListAsync(string path, bool files, string? prefix, int? segmentSize);
        public Task<string> ReadFileToBlobAsync(string strFileName);
    }
}

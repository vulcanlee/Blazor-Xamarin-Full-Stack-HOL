using Domains.Models;

namespace Backend.Services
{
    public interface IExportDataService
    {
        BackendDBContext Context { get; }
        InitDatas InitDatas { get; set; }

        Task CollectionRecord();
        Task<string> GetCollectionJson();
        Task WriteToDirectoryAsync(string path);
    }
}
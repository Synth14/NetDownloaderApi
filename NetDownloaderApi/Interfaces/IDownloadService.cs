using Microsoft.AspNetCore.Mvc;

namespace NetDownloaderApi.Interfaces
{
    public interface IDownloadService
    {
        Task DownloadAsync(string link);
        Task<MemoryStream> DownloadLargeFileAsync(string link);
    }
}
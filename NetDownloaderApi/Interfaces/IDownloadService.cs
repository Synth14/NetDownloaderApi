using Microsoft.AspNetCore.Mvc;

namespace NetDownloaderApi.Interfaces
{
    public interface IDownloadService
    {
        Task DownloadAsync(string link);
        Task DownloadLargeFileAsync(string link);
    }
}
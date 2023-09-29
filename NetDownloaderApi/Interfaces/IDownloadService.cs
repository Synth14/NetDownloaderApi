using Microsoft.AspNetCore.Mvc;

namespace NetDownloaderApi.Interfaces
{
    public interface IDownloadService
    {
        Task<string> DownloadLargeFileAsync(string link, string fileName);
    }
}
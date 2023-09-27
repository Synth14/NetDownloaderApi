using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetDownloaderApi.Interfaces;
using NetDownloaderApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace NetDownloaderApi.Controllers
{
    
    public class DownloadController : ControllerBase
    {
        private readonly IDownloadService _downloadService;
        private readonly IOptions<DownloadConfiguration> _downloadConfiguration;


        public DownloadController(IDownloadService downloadService, IOptions<DownloadConfiguration> downloadConfiguration)
        {
            _downloadService = downloadService;
            _downloadConfiguration = downloadConfiguration;
        }

        [ApiVersion("1")]
        [HttpGet("Download/{link}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Download a file from a URL", Description = "Downloads the file at the specified URL to the client's machine.")]
        public async Task DownloadAsync(string link)
        {
            await _downloadService.DownloadAsync(link);

            // Return a response to the client.
        }

        [HttpGet("DownloadLargeFile/{link}")]
        public async Task<IActionResult> DownloadLargeFile(string link)
        {
            // Replace with the actual URL of the large file you want to download

            try
            {
                await _downloadService.DownloadLargeFileAsync(link);
                var appDirectory = AppContext.BaseDirectory;
                var downloadPath = Path.Combine(appDirectory, "Downloads");

                // Check if the directory exists, and create it if not
                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }

                var filePath = Path.Combine(downloadPath, "largefile.mkv");

                // Set the response headers// Specify the desired file name
                var contentType = "application/octet-stream"; // Set the appropriate content type

                // Rewind the MemoryStream to the beginning
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, contentType, filePath);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., file not found, network errors)
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpGet("api/files")]
        public async Task<IActionResult> DownloadFile([FromQuery] string fileUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var contentType = response.Content.Headers.ContentType?.ToString();

                            var localFilePath = _downloadConfiguration.Value.DownloadPath+"\\pouet3.mkv"; // Replace with your desired path
                            using (var fileStream = System.IO.File.Create(localFilePath))
                            {
                                var stream = await response.Content.ReadAsStreamAsync();
                                var buffer = new byte[8192]; // 8KB buffer (you can adjust this size)
                                var bytesRead = 0;

                                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    await fileStream.FlushAsync(); // Flush the buffer to disk

                                }
                            }

                            // Now, you can return the file from the local path if needed
                            return File(System.IO.File.OpenRead(localFilePath), contentType);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., if the URL is invalid or the download fails.
                return BadRequest(ex.Message);
            }
        }

    }

}

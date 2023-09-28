using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetDownloaderApi.Interfaces;
using NetDownloaderApi.Models;
using NetDownloaderApi.Tools;
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
                // Generate a unique temporary file name
                var tempFilePath = Path.Combine(_downloadConfiguration.Value.DownloadPath+"\\", Guid.NewGuid().ToString() + ".crdownload");

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var contentType = response.Content.Headers.ContentType?.ToString();

                            using (var fileStream = System.IO.File.Create(tempFilePath))
                            {
                                var buffer = new byte[8192]; // 8KB buffer (you can adjust this size)
                                var bytesRead = 0;

                                using (var responseStream = await response.Content.ReadAsStreamAsync())
                                {
                                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                    {
                                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    }
                                }
                            }

                            // Rename the temporary file to remove .crdownload extension
                            var finalFilePath = Path.Combine(_downloadConfiguration.Value.DownloadPath, "\\trucr.mkv"); // Adjust the final file path and extension
                            System.IO.File.Move(tempFilePath, finalFilePath);

                            // Now, you can return the file from the final path
                            return File(System.IO.File.OpenRead(finalFilePath), contentType);
                        }
                        else
                        {
                            // Handle download failure, possibly clean up the temporary file
                            System.IO.File.Delete(tempFilePath);
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
        [HttpGet("api/files2")]

        public async Task<IActionResult> DownloadFilez([FromQuery] string fileUrl)
        {
            try
            {
                var fileType = await FileSerializer.IdentifyFileType(fileUrl);

                var finalFileName = await FileSerializer.GetOrCreateFileName(fileUrl);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var contentType = response.Content.Headers.ContentType?.ToString();

                            var tempFileName = Guid.NewGuid().ToString();
                            // Create a unique temporary file with the extracted or default filename
                            var tempFilePath = Path.Combine(_downloadConfiguration.Value.DownloadPath, tempFileName);

                            using (var fileStream = System.IO.File.Create(tempFilePath))
                            {
                                var buffer = new byte[8192]; // 8KB buffer
                                var bytesRead = 0;

                                using (var responseStream = await response.Content.ReadAsStreamAsync())
                                {
                                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                    {
                                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    }
                                }
                            }

                            // Rename the temporary file to remove .crdownload extension
                            var finalFilePath = Path.Combine(_downloadConfiguration.Value.DownloadPath, tempFileName); // Adjust the final file path

                            // Ensure the final directory exists, create it if necessary
                            var finalDirectory = Path.GetDirectoryName(finalFilePath);
                            if (!Directory.Exists(finalDirectory))
                            {
                                Directory.CreateDirectory(finalDirectory);
                            }
                            if(tempFilePath != finalFilePath) 
                                System.IO.File.Move(tempFilePath, finalFilePath);

                            if (System.IO.File.Exists(finalFilePath))
                            {
                                // Now, you can return the file from the final path
                                return File(System.IO.File.OpenRead(finalFilePath), contentType, tempFileName);
                            }
                            else
                            {
                                // Handle renaming failure
                                System.IO.File.Delete(tempFilePath);
                                return NotFound("Failed to rename the file.");
                            }
                        }
                        else
                        {
                            // Handle download failure
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

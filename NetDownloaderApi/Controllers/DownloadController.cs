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
        [ApiVersion("1")]
        [HttpGet("Download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Download a file from a URL", Description = "Downloads the file at the specified URL to the client's machine.")]
        public async Task<IActionResult> DownloadLargeFileAsync(string fileUrl)
        {
            try
            {

                var fileType = await FileSerializer.IdentifyFileType(fileUrl);
                var finalFileName = await FileSerializer.GetOrCreateFileName(fileUrl);
                var downloadedFilePath = await _downloadService.DownloadLargeFileAsync(fileUrl, finalFileName);

                if (System.IO.File.Exists(downloadedFilePath))
                {
                    return File(System.IO.File.OpenRead(downloadedFilePath), fileType.contentType, finalFileName);
                }
                else
                {
                    return NotFound("Failed to download the file.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}

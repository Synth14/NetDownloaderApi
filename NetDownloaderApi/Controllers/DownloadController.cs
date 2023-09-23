using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDownloaderApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace NetDownloaderApi.Controllers
{
    
    public class DownloadController : ControllerBase
    {
        private readonly IDownloadService _downloadService;

        public DownloadController(IDownloadService downloadService)
        {
            this._downloadService = downloadService;
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
                var memoryStream = await _downloadService.DownloadLargeFileAsync(link);

                // Set the response headers
                var fileName = "largefile.mkv"; // Specify the desired file name
                var contentType = "application/octet-stream"; // Set the appropriate content type

                // Rewind the MemoryStream to the beginning

                return File(memoryStream, contentType, fileName);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., file not found, network errors)
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }

}

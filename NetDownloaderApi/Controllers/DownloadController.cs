using Microsoft.AspNetCore.Mvc;
using NetDownloaderApi.Interfaces;
using NetDownloaderApi.Tools;
using Swashbuckle.AspNetCore.Annotations;


namespace NetDownloaderApi.Controllers
{

    public class DownloadController : ControllerBase
    {
        private readonly IDownloadService _downloadService;


        public DownloadController(IDownloadService downloadService)
        {
            _downloadService = downloadService;
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

                var fileType = await FileSerializer.IdentifyFileType(fileUrl);//will probably used after the clipboard crawler got a link
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

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetDownloader.Entity.Models;
using NetDownloader.Entity.Services;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace NetDownloader.Entity.Controllers
{
    public class LinksController:Controller
    {
        private readonly ILinksService _linksService;

        public LinksController(ILinksService linksService)
        {
            _linksService = linksService;
        }
        [ApiVersion("1")]
        [HttpPost("Createlink")]
        [SwaggerOperation(Summary = "Download a file from a URL", Description = "Downloads the file at the specified URL to the client's machine.")]
        public async Task<IActionResult> CreateLink(Links link)
        {
                var result = await _linksService.CreateLinkAsync(link);
                return Ok(result);
        }
        [ApiVersion("1")]
        [HttpGet("GetAlllinks")]
        [SwaggerOperation(Summary = "Get all links", Description = "Get all links")]
        public async Task<IActionResult> GetAllTags()
        {
            var allTags = await _linksService.GetAllLinksAsync();
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpPatch("Updatelink")]
        [SwaggerOperation(Summary = "Update a link", Description = "Update a link")]
        public async Task<IActionResult> UpdateLink(Links link)
        {
            var allTags = await _linksService.UpdateLinkAsync(link);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpGet("GetlinkById")]
        [SwaggerOperation(Summary = "Get link By Id", Description = "GetlinkById")]
        public async Task<IActionResult> GetLinkById(int id)
        {
            var allTags = await _linksService.GetLinkByIdAsync(id);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpDelete("DeletelinkById")]
        [SwaggerOperation(Summary = "Delete link By Id", Description = "DeletelinkById")]
        public async Task<IActionResult> DeleteLinkById(int id)
        {
            var allTags = await _linksService.DeleteLinkAsync(id);
            return Ok(allTags);
        }
    }
}

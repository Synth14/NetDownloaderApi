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

        public async Task<IActionResult> Create([Bind("HostId,Hostname,AccountId")] Hosts host)
        {
                await _linksService.CreateLinksAsync(host);
                return RedirectToAction(nameof(Index));
            
        }
    }
}

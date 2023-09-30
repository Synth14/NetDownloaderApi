using Microsoft.AspNetCore.Mvc;
using NetDownloader.Entity.Models;
using NetDownloader.Entity.Services;

namespace NetDownloader.Entity.Controllers
{
    public class LinksController:Controller
    {
        private readonly ILinksService _linksService;

        public LinksController(ILinksService linksService)
        {
            _linksService = linksService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HostId,Hostname,AccountId")] Hosts host)
        {
            if (ModelState.IsValid)
            {
                await _linksService.CreateLinksAsync(host);
                return RedirectToAction(nameof(Index));
            }
            return View(host);
        }
    }
}

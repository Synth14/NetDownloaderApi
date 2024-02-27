using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NetDownloader.Entity.Interfaces;
using NetDownloader.Entity.Models;
using NetDownloader.Entity.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NetDownloader.Entity.Controllers
{
    public class HostsController:Controller
    {
        private readonly IHostsService _hostsService;

        public HostsController(IHostsService hostsService)
        {
            _hostsService = hostsService;
        }
        [ApiVersion("1")]
        [HttpPost("CreateHost")]
        [SwaggerOperation(Summary = "CreateHost", Description = "CreateHost")]
        public async Task<IActionResult> CreateHost(Hosts host)
        {
                var result = await _hostsService.CreateHostAsync(host);
                return Ok(result);
        }
        [ApiVersion("1")]
        [HttpGet("GetAllHosts")]
        [SwaggerOperation(Summary = "GetAllHosts", Description = "GetAllHosts")]
        public async Task<IActionResult> GetAllHosts()
        {
            var allTags = await _hostsService.GetAllHostsAsync();
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpPut("UpdateHost")]
        [SwaggerOperation(Summary = "UpdateHost", Description = "UpdateHost")]
        public async Task<IActionResult> UpdateHost(Hosts host)
        {
            var allTags = await _hostsService.UpdateHostAsync(host);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpGet("GetHostById")]
        [SwaggerOperation(Summary = "GetHostById", Description = "GetHostById")]
        public async Task<IActionResult> GetHostById(int id)
        {
            var allTags = await _hostsService.GetHostByIdAsync(id);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpDelete("DeleteHostById")]
        [SwaggerOperation(Summary = "Delete link By Id", Description = "DeletelinkById")]
        public async Task<IActionResult> DeleteHostById(int id)
        {
            var allTags = await _hostsService.DeleteHostByIdAsync(id);
            return Ok(allTags);
        }
    }
}

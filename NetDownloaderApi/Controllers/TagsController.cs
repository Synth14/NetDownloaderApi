using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetDownloader.Entity.Models;
using NetDownloader.Entity.Services;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace NetDownloader.Entity.Controllers
{
    public class TagsController:Controller
    {
        private readonly ITagsService _tagsService;


        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }
        [ApiVersion("1")]
        [HttpPost("CreateTag")]
        [SwaggerOperation(Summary = "Create tag", Description = "Create a new tag")]

        public async Task<IActionResult> Create(Tags tags)
        {
                await _tagsService.CreateTagsAsync(tags);
                return RedirectToAction(nameof(Index));
            
        }
    }
}

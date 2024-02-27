using Microsoft.AspNetCore.Mvc;
using NetDownloader.Entity.Models;
using NetDownloader.Entity.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NetDownloader.Entity.Controllers
{
    public class TagsController : Controller
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
            var result = await _tagsService.CreateTagsAsync(tags);
            return Ok(result);
        }

        [ApiVersion("1")]
        [HttpGet("GetAllTags")]
        [SwaggerOperation(Summary = "Get all tags", Description = "Get all tags")]
        public async Task<IActionResult> GetAllTags()
        { 
            var allTags = await _tagsService.GetAllTagsAsync();
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpPatch("UpdateTag")]
        [SwaggerOperation(Summary = "Update a tag", Description = "Update a tag")]
        public async Task<IActionResult> UpdateTagAsync(Tags tag)
        {
            var allTags = await _tagsService.UpdateTagAsync(tag);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpGet("GetTagById")]
        [SwaggerOperation(Summary = "Get Tag By Id", Description = "GetTagById")]
        public async Task<IActionResult> GetTagsById(int id)
        {
            var allTags = await _tagsService.GetTagsByIdAsync(id);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpDelete("DeleteTagById")]
        [SwaggerOperation(Summary = "Delete Tag By Id", Description = "DeleteTagById")]
        public async Task<IActionResult> DeleteTagById(int id)
        {
            var allTags = await _tagsService.DeleteTagByIdAsync(id);
            return Ok(allTags);
        }
    }
}

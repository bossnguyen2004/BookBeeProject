using BookBee.DTOs.Tag;
using BookBee.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpGet("{id}")]
        public IActionResult GetTagById(int id)
        {
            var res = _tagService.GetCategorysById(id);
            return StatusCode(res.Code, res);
        }
        [HttpGet]
        public IActionResult GetTags(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var res = _tagService.GetCategorys(page, pageSize, key, sortBy);
            return StatusCode(res.Code, res);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateTag(int id, TagRequestDTO tagRequestDto)
        {
            var res = _tagService.UpdateCategorys(id, tagRequestDto);
            return StatusCode(res.Code, res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteTag(int id)
        {
            var res = _tagService.DeleteCategorys(id);
            return StatusCode(res.Code, res);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateTag([FromBody] TagRequestDTO tagRequestDto)
        {
           
            var res = _tagService.CreateCategorys(tagRequestDto.Name, tagRequestDto.Image);
            return StatusCode(res.Code, res);
        }
    }
}

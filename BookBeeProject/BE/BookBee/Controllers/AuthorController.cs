using BookBee.DTOs.Author;
using BookBee.Services.AuthorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet("{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var res = _authorService.GetAuthorById(id);
            return StatusCode(res.Code, res);
        }
        [HttpGet]
        public IActionResult GetAuthors(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var res = _authorService.GetAuthors(page, pageSize, key, sortBy);
            return StatusCode(res.Code, res);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateAuthor(int id, [FromBody] AuthorDTO authorDto)
        {
            var res = _authorService.UpdateAuthor(id, authorDto.Name, authorDto.DeathYear);
            return StatusCode(res.Code, res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteAuthor(int id)
        {
            var res = _authorService.DeleteAuthor(id);
            return StatusCode(res.Code, res);
        }
       
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateAuthor([FromBody] AuthorDTO authorDto)
        {
            var res = _authorService.CreateAuthor(authorDto.Name, authorDto.DateOfBirth, authorDto.DeathYear);
            return StatusCode(res.Code, res);
        }
    }
}

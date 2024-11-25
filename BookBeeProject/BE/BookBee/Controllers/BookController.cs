using BookBee.DTOs.Book;
using BookBee.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var res = _bookService.GetBookById(id);
            return StatusCode(res.Code, res);
        }
        [HttpPost("ids")]
        public IActionResult GetBookByIds(List<int> ids)
        {
            var res = _bookService.GetBookByIds(ids);
            return StatusCode(res.Code, res);
        }
        [HttpGet]
        public IActionResult GetBooks(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", int? tagId = 0, bool includeDeleted = false, int? publisherId = null, int? authorId = null)
        {
            var res = _bookService.GetBooks(page, pageSize, key, sortBy, tagId, includeDeleted, publisherId, authorId);
            return StatusCode(res.Code, res);
        }

        [HttpGet("Recommendations")]
        public IActionResult GetBookRecommendations()
        {
            var res = _bookService.GetBookRecommendations();
            return StatusCode(res.Code, res);
        }
       
        [HttpGet("TopOrdered")]
        public IActionResult GetTopOrderedBooks(int topCount = 10)
        {
            var res = _bookService.GetTopOrderedBooks(topCount);
            return StatusCode(res.Code, res);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateBook(int id, UpdateBookDTO updateBookDTO)
        {
            var res = _bookService.UpdateBook(id, updateBookDTO);
            return StatusCode(res.Code, res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteBook(int id)
        {
            var res = _bookService.DeleteBook(id);
            return StatusCode(res.Code, res);
        }
        [HttpPost("id")]
        [Authorize(Roles = "admin")]
        public IActionResult RestoreBook(int id)
        {
            var res = _bookService.RestoreBook(id);
            return StatusCode(res.Code, res);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateBook([FromBody] BookDTO bookRequestDto)
        {
            if (bookRequestDto == null)
            {
                return BadRequest(new { Code = 400, Message = "Dữ liệu yêu cầu không hợp lệ." });
            }

            if (bookRequestDto.TagIds == null || !bookRequestDto.TagIds.Any())
            {
                return BadRequest(new { Code = 400, Message = "Danh sách TagId không được để trống." });
            }

            // Kiểm tra các tham số đầu vào cần thiết khác
            if (string.IsNullOrEmpty(bookRequestDto.Title))
            {
                return BadRequest(new { Code = 400, Message = "Tiêu đề sách không được để trống." });
            }

            // Kiểm tra các giá trị cần thiết khác, ví dụ: publisher, author, price...
            if (bookRequestDto.PublisherId <= 0)
            {
                return BadRequest(new { Code = 400, Message = "Nhà xuất bản không hợp lệ." });
            }

            if (bookRequestDto.AuthorId <= 0)
            {
                return BadRequest(new { Code = 400, Message = "Tác giả không hợp lệ." });
            }

            var res = _bookService.CreateBook(
                bookRequestDto.Title,
                bookRequestDto.Description,
                bookRequestDto.NumberOfPages,
                bookRequestDto.PublishDate,
                bookRequestDto.Language,
                bookRequestDto.Count,
                bookRequestDto.Price,
                bookRequestDto.Image,
                bookRequestDto.Format,
                bookRequestDto.PageSize,
                bookRequestDto.PublisherId,
                bookRequestDto.AuthorId,
                bookRequestDto.TagIds
            );

            if (res == null)
            {
                return StatusCode(500, new { Code = 500, Message = "Đã xảy ra lỗi khi tạo sách." });
            }

            if (res.Code == 200)
            {
                return Ok(res);  // Trả về mã thành công
            }
            else
            {
                return StatusCode(res.Code, res);  // Trả về mã lỗi
            }
        }
    }
}

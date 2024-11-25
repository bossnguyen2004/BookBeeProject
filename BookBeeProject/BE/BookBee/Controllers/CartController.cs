using BookBee.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{userId}")]
        public IActionResult GetCartByUser(int userId)
        {
            var res = _cartService.GetCartByUser(userId);
            return StatusCode(res.Code, res);
        }
        [HttpGet("Self")]
        public IActionResult GetSelfCart()
        {
            var res = _cartService.GetSelfCart();
            return StatusCode(res.Code, res);
        }
        
        [HttpPut("{userId}")]
        public IActionResult AddToCart(int userId, int bookId, int count)
        {
            var res = _cartService.AddToCart(userId, bookId, count);
            return StatusCode(res.Code, res);
        }

        [HttpPut("Self")]
        public IActionResult SelfAddToCart(int bookId, int count)
        {
            var res = _cartService.SelfAddToCart(bookId, count);
            return StatusCode(res.Code, res);
        }
    }
}

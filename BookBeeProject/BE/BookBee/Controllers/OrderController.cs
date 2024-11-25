using BookBee.DTOs.Order;
using BookBee.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var res = _orderService.GetOrderById(id);
            return StatusCode(res.Code, res);
        }
        [HttpGet]
        public IActionResult GetOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", string? status = "")
        {
            var res = _orderService.GetOrders(page, pageSize, key, sortBy, status);
            return StatusCode(res.Code, res);
        }
        [HttpGet("History")]
        public IActionResult GetHistoryOrders(int userId, int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var res = _orderService.GetOrderByUser(userId, page, pageSize, key, sortBy);
            return StatusCode(res.Code, res);
        }
        [HttpGet("Self")]
        public IActionResult GetSelfHistoryOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var res = _orderService.GetSelfOrders(page, pageSize, key, sortBy);
            return StatusCode(res.Code, res);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDTO updateOrderDTO)
        {
            var res = await _orderService.UpdateOrder(id, updateOrderDTO);
            return StatusCode(res.Code, res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteOrder(int id)
        {
            var res = _orderService.DeleteOrder(id);
            return StatusCode(res.Code, res);
        }
        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDTO createOrderDTO)
        {
            var res = _orderService.CreateOrder(createOrderDTO);
            return StatusCode(res.Code, res);
        }

        [HttpPost("Self")]
        public IActionResult SelfCreateOrder(SelfCreateOrderDTO selfCreateOrderDTO)
        {
            var res = _orderService.SelfCreateOrder(selfCreateOrderDTO);
            return StatusCode(res.Code, res);
        }
    }
}

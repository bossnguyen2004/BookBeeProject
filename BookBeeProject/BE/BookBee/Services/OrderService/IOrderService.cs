using BookBee.DTOs.Order;
using BookBee.DTOs.Response;

namespace BookBee.Services.OrderService
{
    public interface IOrderService
    {
        ResponseDTO GetOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", string? status = "");
        ResponseDTO GetOrderById(int id);
        Task<ResponseDTO> UpdateOrder(int id, UpdateOrderDTO updateOrderDTO);
        ResponseDTO DeleteOrder(int id);
        ResponseDTO CreateOrder(CreateOrderDTO createOrderDTO);
        ResponseDTO SelfCreateOrder(SelfCreateOrderDTO selfCreateOrderDTO);
        ResponseDTO GetOrderByUser(int userId, int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID");
        ResponseDTO GetSelfOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID");
    }
}

using BookBee.Model;

namespace BookBee.Persistences.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        List<Order> GetOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", string? status = "");
        List<Order> GetOrderByUser(int userId, int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", string? status = "");
        Order GetOrderById(int id);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
        void CreateOrder(Order order);
        int GetOrderCount();
        int GetOrderCountByUser(int userId);
        bool IsSaveChanges();
        public int Total { get; }
    }
}

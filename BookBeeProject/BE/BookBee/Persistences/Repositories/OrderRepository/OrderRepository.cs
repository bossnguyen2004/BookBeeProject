using BookBee.Model;
using Microsoft.EntityFrameworkCore;

namespace BookBee.Persistences.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _dataContext;
        public OrderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public int Total { get; set; }
        public void CreateOrder(Order order)
        {
            _dataContext.Orders.Add(order);
        }

        public void DeleteOrder(Order order)
        {
            _dataContext.Orders.Remove(order);
        }

        public Order GetOrderById(int id)
        {
            return _dataContext.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderDetails)
                .ThenInclude(s => s.Book)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<Order> GetOrderByUser(int userId, int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", string? status = "")
        {

            if (!_dataContext.Orders.Any()) return Enumerable.Empty<Order>().ToList();

            var query = _dataContext.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderDetails)
                .ThenInclude(s => s.Book)
                .AsSplitQuery()
                .Where(o => o.User.Id == userId).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
            }

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(au => au.Id == int.Parse(key) || au.User.Username.Contains(key) || au.OrderDetails.Any(b => b.Book.Title.Contains(key)));
            }

            switch (sortBy)
            {
                case "CREATE":
                    query = query.OrderBy(u => u.Create).ThenByDescending(u => u.Id);
                    break;
                case "CREATE_DESC":
                    query = query.OrderByDescending(u => u.Create).ThenByDescending(u => u.Id);
                    break;
                default:
                    query = query.OrderBy(u => u.IsDeleted).ThenByDescending(u => u.Id);
                    break;
            }

            if (page == null || pageSize == null || sortBy == null) { return query.ToList(); }
            else
                return query.Where(o => o.IsDeleted == false).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        public int GetOrderCount()
        {
            return _dataContext.Orders.Count();
        }

        public int GetOrderCountByUser(int userId)
        {
            return _dataContext.Orders.Count(o => o.UserId == userId && o.IsDeleted == false);
        }

        public List<Order> GetOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", string? status = "")
        {
            if (!_dataContext.Orders.Any()) return Enumerable.Empty<Order>().ToList();
            var query = _dataContext.Orders.Where(r => r.IsDeleted == false)
                .Include(o => o.User).Include(o => o.Address)
                .Include(o => o.OrderDetails)
                .ThenInclude(s => s.Book).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(au => au.Status == status);
            }

            if (!string.IsNullOrEmpty(key))
            {
                if (int.TryParse(key, out int id))
                {
                    query = query.Where(au => au.Id == id);
                }
            }

            switch (sortBy)
            {
                case "CREATE":
                    query = query.OrderBy(u => u.Create).ThenBy(u => u.Id);
                    break;
                case "CREATE_DESC":
                    query = query.OrderByDescending(u => u.Create).ThenBy(u => u.Id);
                    break;
                default:
                    query = query.OrderBy(u => u.IsDeleted).ThenBy(u => u.Id);
                    break;
            }

            Total = query.Count();

            if (page == null || pageSize == null || sortBy == null) { return query.ToList(); }

            return query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void UpdateOrder(Order order)
        {
            order.Update = DateTime.Now;
            _dataContext.Orders.Update(order);
        }
    }
}

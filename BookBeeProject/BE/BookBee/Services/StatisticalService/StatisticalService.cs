using BookBee.DTOs.Response;
using BookBee.Persistences.Repositories.OrderRepository;
using BookBee.Persistences.Repositories.UserRepository;

namespace BookBee.Services.StatisticalService
{
    public class StatisticalService : IStatisticalService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        public StatisticalService(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }
        public object GetStatisticalUser()
        {
            var users = _userRepository.GetUsers(1, 10000, "", "ID");
            int[] arrUser = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DateTime today = DateTime.Today;
            var year = today.Year;
            var month = today.Month;
            var day = today.Day;

            int countUser = 0;
            foreach (var user in users.Where(u => u.RoleId == 2))
            {
                if (user.Create.Year == year)
                {
                    arrUser[user.Create.Month - 1] += 1;
                    countUser++;
                }
            }
            return new
            {
                Total = countUser,
                Statistical = arrUser,
                Diff = $"{(arrUser[month - 2] == 0 ? 0 : ((arrUser[month - 1] - arrUser[month - 2]) / arrUser[month - 2]) * 100):F2}",
            };
        }

        public object GetStatisticalOrder()
        {
            var orders = _orderRepository.GetOrders(1, 10000, "", "ID");
            int[] arrOrder = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DateTime today = DateTime.Today;
            var year = today.Year;
            var month = today.Month;
            var day = today.Day;

            int countOrder = 0;
            foreach (var order in orders)
            {
                if (order.Create.Year == year)
                {
                    arrOrder[order.Create.Month - 1] += 1;
                    countOrder++;
                }
            }
            return new
            {
                Total = countOrder,
                Statistical = arrOrder,
                Diff = $"{(arrOrder[month - 2] == 0 ? 0 : ((arrOrder[month - 1] - arrOrder[month - 2]) / arrOrder[month - 2]) * 100):F2}"
            };
        }

        public object GetStatisticalRevenue()
        {
            var orders = _orderRepository.GetOrders(1, 10000, "", "ID");
            double[] arrOrder = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DateTime today = DateTime.Today;
            var year = today.Year;
            var month = today.Month;
            var day = today.Day;

            int countOrder = 0;
            foreach (var order in orders)
            {
                if (order.Create.Year == year && order.Status == "DON")
                {
                    double totalMonth = 0;
                    for (int i = 0; i < order.OrderDetails.Count(); i++)
                    {
                        totalMonth += (order.OrderDetails[i].Price * order.OrderDetails[i].Quantity);
                    }
                    arrOrder[order.Create.Month - 1] += totalMonth;
                    countOrder++;
                }
            }
            return new
            {
                Total = countOrder,
                Statistical = arrOrder,
                Diff = $"{(arrOrder[month - 2] == 0 ? 0 : ((arrOrder[month - 1] - arrOrder[month - 2]) / arrOrder[month - 2]) * 100):F2}"
            };
        }
        public ResponseDTO GetStatistical()
        {
            //var statistical;
            var users = _userRepository.GetUsers(1, 10000, "", "ID");
            return new ResponseDTO()
            {
                Data = new
                {
                    User = GetStatisticalUser(),
                    Order = GetStatisticalOrder(),
                    Revenue = GetStatisticalRevenue(),
                }
            };
        }
    }
}

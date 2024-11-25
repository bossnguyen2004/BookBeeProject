using AutoMapper;
using BookBee.DTOs.Order;
using BookBee.DTOs.OrderDetail;
using BookBee.DTOs.Response;
using BookBee.Model;
using BookBee.Persistences.Repositories.AddressRepository;
using BookBee.Persistences.Repositories.BookRepository;
using BookBee.Persistences.Repositories.CartRepository;
using BookBee.Persistences.Repositories.OrderRepository;
using BookBee.Persistences.Repositories.QuantityRepository;
using BookBee.Persistences.Repositories.UserRepository;
using BookBee.Services.MailService;
using BookBee.Utilities;
using System.Text;

namespace BookBee.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IAddressRepository _addressRepository;
        private readonly IQuantityRepository _quantityRepository;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly UserAccessor _userAccessor;



        public OrderService(IOrderRepository orderRepository, IMapper mapper, IBookRepository bookRepository,
            IUserRepository userRepository, IAddressRepository addressRepository, IQuantityRepository quantityRepository, ICartRepository cartRepository,
            UserAccessor userAccessor, IMailService mailService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _quantityRepository = quantityRepository;
            _addressRepository = addressRepository;
            _cartRepository = cartRepository;
            _userAccessor = userAccessor;
            _mailService = mailService;
        }

        public ResponseDTO CreateOrder(CreateOrderDTO createOrderDTO)
        {
            var user = _userRepository.GetUserById(createOrderDTO.UserId);
            if (user == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "User không tồn tại"
            };

            var address = _addressRepository.GetAddressById(createOrderDTO.AddressId);
            if (address == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "Address không tồn tại"
            };

            if (user.Addresses.IndexOf(address) < 0)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Địa chỉ không hợp lệ"
                };
            }

            var order = _mapper.Map<Order>(createOrderDTO);

            for (int i = 0; i < createOrderDTO.BookIds.Count; i++)
            {
                var book = _bookRepository.GetBookById(createOrderDTO.BookIds[i]);
                if (book != null)
                {
                    if (book.IsDeleted)
                    {
                        return new ResponseDTO()
                        {
                            Code = 400,
                            Message = $"Sách {book.Title} hiện không có sẵn"
                        };
                    }

                    if (book.Count < createOrderDTO.QuantitieCounts[i])
                    {
                        return new ResponseDTO()
                        {
                            Code = 400,
                            Message = $"Không đủ số lướng cho sách {book.Title}"
                        };
                    }
                    order.OrderDetails.Add(new OrderDetail()
                    {
                        BookId = book.Id,
                        Quantity = createOrderDTO.QuantitieCounts[i],
                        Price = book.Price
                    });
                }
            }

            _orderRepository.CreateOrder(order);

            if (_orderRepository.IsSaveChanges())
            {
                var orderId = order.Id;

                foreach (var orderBook in order.OrderDetails)
                {
                    var book = _bookRepository.GetBookById(orderBook.BookId);
                    if (book != null)
                    {
                        if (book.Count >= orderBook.Quantity)
                        {
                            book.Count -= orderBook.Quantity;
                            _bookRepository.UpdateBook(book);
                        }
                        else
                        {
                            return new ResponseDTO()
                            {
                                Code = 400,
                                Message = $"Không đủ số lượng cho sách ID {book.Id}"
                            };
                        }
                    }
                }

                if (_bookRepository.IsSaveChanges())
                {
                    _cartRepository.ClearCartBook(order.OrderDetails.Select(c => c.BookId).ToList());

                    return new ResponseDTO()
                    {
                        Message = "Tạo thành công",
                        Data = orderId 
                    };
                }

                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Cập nhật số lượng sách thất bại"
                };
            }

            return new ResponseDTO()
            {
                Code = 400,
                Message = "Tạo thất bại"
            };
        }

        public ResponseDTO SelfCreateOrder(SelfCreateOrderDTO selfCreateOrderDTO)
        {
            var userId = _userAccessor.GetCurrentUserId();
            if (userId != null)
            {
                var user = _userRepository.GetUserById((int)userId);
                if (user == null)
                    return new ResponseDTO()
                    {
                        Code = 400,
                        Message = "User không tồn tại"
                    };


                var address = _addressRepository.GetAddressById(selfCreateOrderDTO.AddressId);
                if (address == null)
                    return new ResponseDTO()
                    {
                        Code = 400,
                        Message = "Address không tồn tại"
                    };

                if (user.Addresses.IndexOf(address) < 0)
                {
                    return new ResponseDTO()
                    {
                        Code = 400,
                        Message = "Địa chỉ không hợp lệ"
                    };
                }

                var order = _mapper.Map<Order>(selfCreateOrderDTO);
                order.UserId = (int)userId;

                for (int i = 0; i < selfCreateOrderDTO.BookIds.Count; i++)
                {
                    var book = _bookRepository.GetBookById(selfCreateOrderDTO.BookIds[i]);
                    if (book != null)
                    {
                        if (book.IsDeleted)
                        {
                            return new ResponseDTO()
                            {
                                Code = 400,
                                Message = $"Sách {book.Title} hiện không có sẵn"
                            };
                        }

                        if (book.Count < selfCreateOrderDTO.QuantitieCounts[i])
                        {
                            return new ResponseDTO()
                            {
                                Code = 400,
                                Message = $"Không đủ số lướng cho sách {book.Title}"
                            };
                        }
                        order.OrderDetails.Add(new OrderDetail()
                        {
                            BookId = book.Id,
                            Quantity = selfCreateOrderDTO.QuantitieCounts[i],
                            Price = book.Price
                        });
                    }
                }

                _orderRepository.CreateOrder(order);

                if (_orderRepository.IsSaveChanges())
                {
                    var orderId = order.Id;

                    foreach (var orderBook in order.OrderDetails)
                    {
                        var book = _bookRepository.GetBookById(orderBook.BookId);
                        if (book != null)
                        {
                            if (book.Count >= orderBook.Quantity)
                            {
                                book.Count -= orderBook.Quantity;
                                _bookRepository.UpdateBook(book);
                            }
                            else
                            {
                                return new ResponseDTO()
                                {
                                    Code = 400,
                                    Message = $"Không đủ số lượng cho sách ID {book.Id}"
                                };
                            }
                        }
                    }
                    if (_bookRepository.IsSaveChanges())
                    {
                        _cartRepository.ClearCartBook(order.OrderDetails.Select(c => c.BookId).ToList());

                        return new ResponseDTO()
                        {
                            Message = "Tạo thành công",
                            Data = orderId 
                        };
                    }

                    return new ResponseDTO()
                    {
                        Code = 400,
                        Message = "Cập nhật số lượng sách thất bại"
                    };
                }

                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Tạo thất bại"
                };
            }

            return new ResponseDTO
            {
                Code = 404,
                Message = "User không tồn tại"
            };
        }

        public ResponseDTO DeleteOrder(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Order không tồn tại"
                };

            order.IsDeleted = true;
            _orderRepository.UpdateOrder(order);
            if (_orderRepository.IsSaveChanges())
                return new ResponseDTO()
                {
                    Message = "Xóa thành công"
                };
            else return new ResponseDTO()
            {
                Code = 400,
                Message = "Xóa thất bại"
            };
        }

        public ResponseDTO GetOrderByUser(int userId, int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "User không tồn tại"
            };

            var orders = _orderRepository.GetOrderByUser(userId, page, pageSize, key, sortBy);
            return new ResponseDTO()
            {
                Code = 200,
                Data = _mapper.Map<List<OrderDTO>>(orders),
                Total = _orderRepository.GetOrderCountByUser(userId)
            };
        }

        public ResponseDTO GetSelfOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var userId = _userAccessor.GetCurrentUserId();
            if (userId != null) return GetOrderByUser((int)userId, page, pageSize, key, sortBy);
            return new ResponseDTO()
            {
                Code = 400,
                Message = "User không tồn tại"
            };
        }

        public ResponseDTO GetOrderById(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Order không tồn tại"
                };

            OrderDTO orderDTO = _mapper.Map<OrderDTO>(order);
            List<OrderDetailDTO> tmp = _mapper.Map<List<OrderDetailDTO>>(order.OrderDetails);
            orderDTO.OrderDetails = tmp;
            orderDTO.TotalPrice = tmp.Sum(b => b.Price * b.Quantity);
            return new ResponseDTO
            {
                Data = orderDTO
            };
        }

        public ResponseDTO GetOrders(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID", string? status = "")
        {
            var orders = _orderRepository.GetOrders(page, pageSize, key, sortBy, status);
            var tmp = _mapper.Map<List<OrderDTO>>(orders);
            tmp.ForEach(c => c.TotalPrice = c.OrderDetails.Sum(b => b.Price * b.Quantity));
            return new ResponseDTO()
            {
                Data = tmp,
                Total = _orderRepository.Total
            };
        }

        public async Task<ResponseDTO> UpdateOrder(int id, UpdateOrderDTO updateOrderDTO)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Order không tồn tại"
                };

            // Update order details
            order.Update = DateTime.Now;
            order.Status = updateOrderDTO.Status;
            order.Description = updateOrderDTO.Description;

            _orderRepository.UpdateOrder(order);
            if (_orderRepository.IsSaveChanges())
            {
                if (order.Status == "DON")
                {
                    var user = _userRepository.GetUserById(order.UserId);
                    if (user != null && !string.IsNullOrEmpty(user.Email) && order.Type == "ONLINE")
                    {
                        var invoiceContent = GenerateInvoice(order);
                        var invoiceEmail = GenerateInvoiceEmail(invoiceContent);
                        await _mailService.SendEmailAsync(user.Email, "Hóa đơn đơn hàng từ cửa hàng sách BookBee", invoiceEmail);
                    }
                }

                if (order.Status == "CAN")
                {
                    foreach (var orderBook in order.OrderDetails)
                    {
                        var book = _bookRepository.GetBookById(orderBook.BookId);
                        book.Count += orderBook.Quantity;
                        _bookRepository.UpdateBook(book);
                    }

                    _bookRepository.IsSaveChanges();
                }

                return new ResponseDTO()
                {
                    Code = 200,
                    Message = "Cập nhật đơn hàng thành công"
                };
            }

            return new ResponseDTO()
            {
                Code = 400,
                Message = "Cập nhật đơn hàng thất bại"
            };
        }

        private string GenerateInvoice(Order order)
        {
            var invoiceBuilder = new StringBuilder();

            invoiceBuilder.AppendLine($"<h2>Hóa đơn cho đơn hàng #{order.Id}</h2>");
            invoiceBuilder.AppendLine("<p><strong>Ngày đặt hàng:</strong> " + order.Create.ToString("dd/MM/yyyy HH:mm") + "</p>");
            invoiceBuilder.AppendLine("<br/>");
            invoiceBuilder.AppendLine("<h3>Thông tin khách hàng:</h3>");
            invoiceBuilder.AppendLine($"<p><strong>Họ và tên:</strong> {order.User.FirstName} {order.User.LastName}</p>");
            invoiceBuilder.AppendLine($"<p><strong>Email:</strong> {order.User.Email}</p>");
            invoiceBuilder.AppendLine($"<p><strong>Số điện thoại:</strong> {order.User.Phone}</p>");
            invoiceBuilder.AppendLine("<br/>");

            invoiceBuilder.AppendLine("<h3>Địa chỉ giao hàng:</h3>");
            invoiceBuilder.AppendLine($"<p><strong>Tên người nhận:</strong> {order.Address.Name}</p>");
            invoiceBuilder.AppendLine($"<p><strong>Địa chỉ:</strong> {order.Address.Street}, {order.Address.City}, {order.Address.State}</p>");
            invoiceBuilder.AppendLine($"<p><strong>Số điện thoại:</strong> {order.Address.Phone}</p>");
            invoiceBuilder.AppendLine("<br/>");

            invoiceBuilder.AppendLine("<h3>Chi tiết đơn hàng:</h3>");
            invoiceBuilder.AppendLine("<table style='width:100%; border-collapse:collapse;'>");
            invoiceBuilder.AppendLine("<thead><tr><th>Tên sách</th><th>Số lượng</th><th>Đơn giá</th><th>Tổng cộng</th></tr></thead>");
            invoiceBuilder.AppendLine("<tbody>");

            double totalAmount = 0;
            foreach (var orderBook in order.OrderDetails)
            {
                var lineTotal = orderBook.Quantity * orderBook.Price;
                invoiceBuilder.AppendLine($"<tr><td>{orderBook.Book.Title}</td><td>{orderBook.Quantity}</td><td>{orderBook.Price.ToString("N0")} VND</td><td>{lineTotal.ToString("N0")} VND</td></tr>");
                totalAmount += lineTotal;
            }

            invoiceBuilder.AppendLine("</tbody></table>");
            invoiceBuilder.AppendLine($"<h3>Tổng cộng: {totalAmount.ToString("N0")} VND</h3>");
            //note line
            invoiceBuilder.AppendLine("<p><strong>Lưu ý: đơn hàng đã bao gồm VAT nhưng chưa bao gồm phí vận chuyển.</strong> </p>");
            invoiceBuilder.AppendLine("<br/>");
            return invoiceBuilder.ToString();
        }

        private string GenerateInvoiceEmail(string invoiceContent)
        {
            return $$"""
                <!DOCTYPE html>
                <html lang="vi">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 20px;
                    }
                    .email-container {
                        background-color: #ffffff;
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                    }
                    .logo {
                       text-align: center;
                       margin-bottom: 10px;
                    }
                    .logo img {
                        width: 120px;
                    }
                    .header {
                        text-align: center;
                        padding: 10px 0;
                        color: #333;
                    }
                    .header img {
                        max-width: 150px;
                        height: auto;
                    }
                    .header h1 {
                        color: #007bff;
                        font-size: 24px;
                    }
                    .content {
                        padding: 20px 0;
                        text-align: left;
                    }
                    .content p {
                        color: #555;
                        line-height: 1.5;
                    }
                    .invoice-content {
                        background-color: #f9f9f9;
                        padding: 15px;
                        border-radius: 5px;
                        border: 1px solid #ddd;
                        overflow-x: auto; /* Allows horizontal scrolling if needed */
                    }
                    table {
                        width: 100%;
                        border-collapse: collapse;
                        margin-bottom: 20px;
                        border: 1px solid #ddd;
                    }
                    th, td {
                        border: 1px solid #ddd;
                        padding: 10px;
                        text-align: left;
                        white-space: nowrap; /* Prevents text from wrapping */
                        overflow: hidden; /* Ensures text overflow is hidden */
                        text-overflow: ellipsis; /* Adds ellipsis for overflowed text */
                    }
                    th {
                        background-color: #555; /* Green background for header */
                        color: white;
                    }
                    .footer {
                        text-align: center;
                        padding: 10px 0;
                        color: #888;
                        font-size: 12px;
                        border-top: 1px solid #dddddd;
                        margin-top: 30px;
                    }
                    /* Responsive design */
                    @media screen and (max-width: 600px) {
                        .invoice-content {
                            padding: 10px;
                        }
                        table {
                            width: 100%;
                            border: none;
                        }
                        th, td {
                            display: block;
                            width: 100%;
                            box-sizing: border-box;
                            white-space: normal; /* Allows text wrapping on small screens */
                        }
                    }
                </style>
                </head>
                <body>
                    <div class="email-container">
                        <div class="logo">
                            <img src="https://i.pinimg.com/736x/36/24/e6/3624e650ec342dd00e8bf2b05ead4062.jpg" alt="BookBee Logo">
                        </div>
                        <div class="header">
                            <h1>Hóa đơn từ BookBee</h1>
                        </div>
                        <div class="content">
                            <p>Chào bạn,</p>
                            <p>Cảm ơn bạn đã đặt hàng từ BookBee. Dưới đây là hóa đơn của bạn:</p>
                            <div class="invoice-content">
                                {{invoiceContent}}
                            </div>
                            <p>Xin cảm ơn bạn đã đặt hàng từ BookBee!</p>
                        </div>
                        <div class="footer">
                            <p>&copy; 2025 BookBee. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            """;
        }
    }
}

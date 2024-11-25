using AutoMapper;
using BookBee.DTOs.Cart;
using BookBee.DTOs.CartDetail;
using BookBee.DTOs.Response;
using BookBee.Model;
using BookBee.Persistences.Repositories.BookRepository;
using BookBee.Persistences.Repositories.CartRepository;
using BookBee.Persistences.Repositories.QuantityRepository;
using BookBee.Persistences.Repositories.UserRepository;
using BookBee.Utilities;

namespace BookBee.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IQuantityRepository _quantityRepository;
        private readonly IMapper _mapper;
        private readonly UserAccessor _userAccessor;
        public CartService(ICartRepository cartRepository, IMapper mapper, IUserRepository userRepository, IQuantityRepository quantityRepository, IBookRepository bookRepository, UserAccessor userAccessor)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _userAccessor = userAccessor;
            _quantityRepository = quantityRepository;
        }

        public ResponseDTO AddToCart(int userId, int bookId, int count)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null) return new ResponseDTO { Code = 400, Message = "Book không tồn tại" };

            var cart = _cartRepository.GetCartByUser(userId);
            if (cart == null) return new ResponseDTO { Code = 400, Message = "Cart của user không tồn tại" };

            if (book.IsDeleted) return new ResponseDTO { Code = 400, Message = "Sản phẩm này hiện không có sẵn" };

            if (count > book.Count) return new ResponseDTO { Code = 400, Message = "Sản phẩm hiện không đủ số lượng để thêm" };

          

            if (cart.CartDetails.FirstOrDefault(b => b.Book.Id == bookId) == null)
            {
                cart.CartDetails.Add(
                    new CartDetail()
                    {
                        CartId = cart.Id,
                        BookId = bookId,
                        Quantity = count
                    });
            }
            else
                for (int i = 0; i < cart.CartDetails.Count; i++)
                {
                    if (cart.CartDetails[i].Book.Id == bookId)
                    {
                        if (cart.CartDetails[i].Quantity + count > book.Count) return new ResponseDTO { Code = 400, Message = "Sản phẩm hiện không đủ số lượng để thêm" };
                    
                        if (cart.CartDetails[i].Quantity + count == 0) cart.CartDetails.RemoveAt(i);
                        else     
                            cart.CartDetails[i].Quantity += count;
                        break;
                    }
                }

            cart.Update = DateTime.Now;
            _cartRepository.UpdateCart(cart);
            if (_cartRepository.IsSaveChanges())
            {
                var tmp = _cartRepository.GetCartByUser(userId);
                return new ResponseDTO()
                {
                    Message = "Cập nhật thành công"
                };
            }
            else return new ResponseDTO()
            {
                Code = 400,
                Message = "Cập nhật thất bại"
            };

        }

        public ResponseDTO SelfAddToCart(int bookId, int count)
        {
            var userId = _userAccessor.GetCurrentUserId();
            if (userId != null) return AddToCart((int)userId, bookId, count);
            return new ResponseDTO
            {
                Code = 400,
                Message = "User không tồn tại"
            };
        }

        public ResponseDTO GetCartByUser(int userId)
        {
            var cart = _cartRepository.GetCartByUser(userId);
            if (cart != null)
            {
                CartDTO cartDTO = new CartDTO();
                List<CartDetailDTO> tmp = _mapper.Map<List<CartDetailDTO>>(cart.CartDetails);
                cartDTO.CartDetails = tmp;
                return new ResponseDTO
                {
                    Data = tmp
                };
            }
            else return new ResponseDTO
            {
                Code = 400,
                Message = "Giỏ hàng của user này không tồn tại"
            };
        }

        public ResponseDTO GetSelfCart()
        {
            var userId = _userAccessor.GetCurrentUserId();
            if (userId != null) return GetCartByUser((int)userId);
            return new ResponseDTO
            {
                Code = 400,
                Message = "User không tồn tại"
            };
        }

      
    }
}

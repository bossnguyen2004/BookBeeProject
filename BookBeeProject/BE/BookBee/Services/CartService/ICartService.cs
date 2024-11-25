using BookBee.DTOs.Response;

namespace BookBee.Services.CartService
{
    public interface ICartService
    {
        ResponseDTO GetCartByUser(int userId);
        ResponseDTO GetSelfCart();
        ResponseDTO AddToCart(int userId, int bookId, int count);
        ResponseDTO SelfAddToCart(int bookId, int count);
    }
}

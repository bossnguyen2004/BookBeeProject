using BookBee.DTOs.CartDetail;

namespace BookBee.DTOs.Cart
{
    public class CartDTO
    {
        public List<CartDetailDTO> CartDetails { get; set; }
        public CartDTO() { }
    }
}

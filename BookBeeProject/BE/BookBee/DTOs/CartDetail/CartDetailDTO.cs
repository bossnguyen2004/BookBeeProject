using BookBee.DTOs.Book;

namespace BookBee.DTOs.CartDetail
{
    public class CartDetailDTO
    {
        public BookDTO Book { get; set; }
        public int Quantity { get; set; }
    }
}

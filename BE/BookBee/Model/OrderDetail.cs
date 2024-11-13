using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBee.Model
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; } = 0;
        public double Price { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public OrderDetail() { }
        public OrderDetail(int quantity, int orderId, int bookId)
        {
            Quantity = quantity;
            OrderId = orderId;
            BookId = bookId;
        }
    }
}

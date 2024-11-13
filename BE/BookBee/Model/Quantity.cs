using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBee.Model
{
    public class Quantity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Count { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();
        public virtual List<Cart> Carts { get; set; } = new List<Cart>();
    }
}

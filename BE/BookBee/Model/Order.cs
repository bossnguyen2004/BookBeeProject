using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBee.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Type { get; set; } = "ONLINE";
        public string Status { get; set; } = "NEW";
        public string PayMode { get; set; } = "CASH";
        public string Description { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
        public DateTime Create { get; set; } = DateTime.Now;
        public DateTime Update { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public virtual User User { get; set; }     
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }



        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}

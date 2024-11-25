using BookBee.DTOs.Address;
using BookBee.DTOs.OrderDetail;
using BookBee.DTOs.User;

namespace BookBee.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string PayMode { get; set; }
        public string Description { get; set; }
        public double TotalPrice { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public virtual UserDTO User { get; set; }
        public virtual AddressDTO Address { get; set; }
        public virtual List<OrderDetailDTO> OrderDetails { get; set; }
    }
}

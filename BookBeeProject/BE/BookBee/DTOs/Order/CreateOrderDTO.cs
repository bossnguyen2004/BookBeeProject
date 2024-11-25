namespace BookBee.DTOs.Order
{
    public class CreateOrderDTO
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string PayMode { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public List<int> QuantitieCounts { get; set; }
        public virtual List<int> BookIds { get; set; }
    }
}

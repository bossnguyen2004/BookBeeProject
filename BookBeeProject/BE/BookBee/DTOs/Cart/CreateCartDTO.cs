namespace BookBee.DTOs.Cart
{
    public class CreateCartDTO
    {
        public List<int> BookIds { get; set; }
        public List<int> QuantitieCounts { get; set; }
    }
}

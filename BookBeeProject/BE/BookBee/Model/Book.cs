using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookBee.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int NumberOfPages { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;
        public string? Language { get; set; }
        public int Count { get; set; } = 0;
        public double Price { get; set; } = 0;
        public int MaxOrder { get; set; } 
        public string? Image { get; set; }
        public string? Format { get; set; }
        public string? PageSize { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public DateTime Update { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public int? PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }
        public int? AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public virtual List<Tag> Tags { get; set; } = new List<Tag>();
        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual List<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}

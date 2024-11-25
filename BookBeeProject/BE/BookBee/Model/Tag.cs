using System.ComponentModel.DataAnnotations;

namespace BookBee.Model
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; } = "";
        public DateTime Create { get; set; } = DateTime.Now;
        public DateTime Update { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}

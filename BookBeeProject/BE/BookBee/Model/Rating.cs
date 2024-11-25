using System.ComponentModel.DataAnnotations;

namespace BookBee.Model
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int Rate { get; set; } = 0;
        public string Comment { get; set; } = "";
        public DateTime Create { get; set; } = DateTime.Now;
        public DateTime Update { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}

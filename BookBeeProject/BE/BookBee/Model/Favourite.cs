using System.ComponentModel.DataAnnotations;

namespace BookBee.Model
{
    public class Favourite
    {
        [Key]
        public int Id { get; set; }
        public int TrangThai { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}

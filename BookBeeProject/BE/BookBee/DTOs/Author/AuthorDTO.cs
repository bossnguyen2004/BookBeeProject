namespace BookBee.DTOs.Author
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DateOfBirth { get; set; }
        public int? DeathYear { get; set; }
        public bool IsDeleted { get; set; }
    }
}

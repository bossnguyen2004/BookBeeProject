namespace BookBee.DTOs.User
{
    public class UpdateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public int? RoleId { get; set; }
        public string? Avatar { get; set; }
    }
}

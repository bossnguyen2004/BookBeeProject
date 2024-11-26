namespace Fe_Admin.Models
{
    public class LoginData
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public bool gender { get; set; }
        public string phone { get; set; }
        public DateTime dob { get; set; }
        public string avatar { get; set; }
        public string username { get; set; }
        public bool isDeleted { get; set; }
        public UserRole role { get; set; } // Thông tin vai trò của người dùng
        public string token { get; set; } // JWT Token
    }
}

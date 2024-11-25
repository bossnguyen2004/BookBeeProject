namespace BookBee.DTOs.User
{
    public class ResetPasswordDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string CfPassword { get; set; }
    }
}

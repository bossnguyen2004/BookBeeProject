﻿namespace Fe_Admin.Models
{
    public class LoginResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public int total { get; set; }
        public LoginData data { get; set; }
    }
}

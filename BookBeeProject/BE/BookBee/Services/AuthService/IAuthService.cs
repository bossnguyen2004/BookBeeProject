﻿using BookBee.DTOs.Response;
using BookBee.DTOs.User;

namespace BookBee.Services.AuthService
{
    public interface IAuthService
    {
        ResponseDTO Login(string username, string password);
        Task<ResponseDTO> Register(RegisterUserDTO registerUserDTO);
        ResponseDTO ChangePassword(ChangePasswordDTO changePasswordDTO);
        Task<ResponseDTO> ForgotPassword(string email);
        ResponseDTO ResetPassword(ResetPasswordDTO resetPasswordDTO);
    }
}

﻿using BookBee.Model;

namespace BookBee.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}

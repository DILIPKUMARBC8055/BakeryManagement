using Microsoft.AspNetCore.Identity;
using System;

namespace BakeryApp.Application.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        
        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<object>(); 
        }

   
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password); 
        }

        
        public bool VerifyPassword(string hashedPassword, string passwordToCheck)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, passwordToCheck);
            return result == PasswordVerificationResult.Success; 
        }
    }
}

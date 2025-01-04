using BakeryApp.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BakeryApp.Application.Response
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
      
        public UserRole Role { get; set; }
    }
}

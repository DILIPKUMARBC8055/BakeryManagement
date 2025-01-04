using System.ComponentModel.DataAnnotations;

namespace BakeryApp.Core.Entities
{
    public enum UserRole
    {
        Admin = 1,
        Owner = 2,
        Customer = 3
    }
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string  Email{ get; set; }
        public string Password{ get; set; }
        public UserRole Role { get; set; }
    }
}

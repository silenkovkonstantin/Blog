using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Contracts.Models.Users
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

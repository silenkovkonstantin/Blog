using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Contracts.Models.Users
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordReg { get; set; }
        public string PasswordConfirm { get; set; }
    }
}

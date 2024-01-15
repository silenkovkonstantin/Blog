using BlogAPI.Contracts.Models.Roles;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Contracts.Models.Users
{
    public class EditUserRequest : AddUserRequest
    {
        public List<string> Roles { get; set; }
    }
}

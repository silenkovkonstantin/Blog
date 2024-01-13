using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Contracts.Models.Users
{
    public class EditUserRequest : UserView
    {
        public string Id { get; set; }
    }
}

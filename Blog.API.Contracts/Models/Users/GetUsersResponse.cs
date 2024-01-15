using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogAPI.Contracts.Models.Roles;

namespace BlogAPI.Contracts.Models.Users
{
    public class GetUsersResponse
    {
        public int UserAmount { get; set; }
        public List<UserView> Users { get; set; }
    }

    public class UserView
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}

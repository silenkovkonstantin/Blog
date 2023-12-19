using Blog.Data.Models.Db;

namespace Blog.ViewModels
{
    public class RoleViewModel
    {
        public Role Role { get; set; }
        public RoleViewModel(Role role)
        {
            Role = role;
        }
    }
}

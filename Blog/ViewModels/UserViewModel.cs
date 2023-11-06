using Blog.Models.Db;

namespace Blog.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }

        public UserViewModel(User user)
        {
            User = user;
        }

        public List<Post> Posts { get; set; }
    }
}

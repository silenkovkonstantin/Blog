using Blog.Data.Models.Db;
using Blog.ViewModels;

namespace Blog.Extensions
{
    public static class UserFromModel
    {
        public static User Convert(this User user, UserViewModel usereditvm)
        {
            //user.ImageUrl = usereditvm.ImageUrl;
            user.Email = usereditvm.Email;

            return user;
        }
    }
}

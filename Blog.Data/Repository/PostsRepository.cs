using Blog.Data.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class PostsRepository : Repository<Post>
    {
        public PostsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Post>> GetAllUserPostsAsync(string id)
        {
            Set.Include(x => x.User);

            var userPosts = Set.AsEnumerable().Where(x => x.User.Id == id).ToList();
            return await Task.Run(() => userPosts);
        }
    }
}

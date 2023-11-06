using Blog.Models.Db;
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
            Set.Include(x => x.Author);

            var userPosts = Set.AsEnumerable().Where(x => x.Author.Id == id).ToList();
            return await Task.Run(() => userPosts);
        }
    }
}

using Blog.Data.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class CommentsRepository : Repository<Comment>
    {
        public CommentsRepository(ApplicationDbContext context) : base(context)
        {

        }



        public async Task<List<Comment>> GetAllPostCommentsAsync(int id)
        {
            Set.Include(x => x.PostId);

            var postComments = Set.AsEnumerable().Where(x => x.PostId == id).ToList();
            return await Task.Run(() => postComments);
        }
    }
}

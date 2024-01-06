using Blog.Data.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class CommentsRepository : Repository<Comment>
    {
        public CommentsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Set<Comment>()
                .Include(x => x.Post)
                .Include(x => x.User)
                .ToArrayAsync();
        }

        public async Task<List<Comment>> GetAllPostCommentsAsync(int id)
        {
            Set.Include(x => x.PostId);

            var postComments = Set.AsEnumerable().Where(x => x.PostId == id).ToList();
            return await Task.Run(() => postComments);
        }

        public override async Task<IEnumerable<Comment>> GetAllByUserIdAsync(string id)
        {
            return await _context.Set<Comment>()
                .Include(x => x.User)
                .Where(p => p.UserId == id)
                .ToArrayAsync();
        }
    }
}

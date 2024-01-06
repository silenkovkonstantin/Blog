using Blog.Data.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class PostsRepository : Repository<Post>
    {
        public PostsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Post> GetAsync(int id)
        {
            //Set.Include(x => x.Comments);
            //Set.Include(x => x.Tags);
            //Set.Include(x => x.User);
            return await _context.Set<Post>()
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .Include(x => x.User)
                .FirstOrDefaultAsync(p => p.Id == id);
                //.FindAsync(id);
        }

        public override async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Set<Post>()
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .Include(x => x.User)
                .ToArrayAsync();
        }

        public override async Task<IEnumerable<Post>> GetAllByUserIdAsync(string id)
        {
            return await _context.Set<Post>()
                .Include(x => x.User)
                .Where(p => p.UserId == id)
                .ToArrayAsync();
        }
    }
}

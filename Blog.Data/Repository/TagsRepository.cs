using Blog.Data.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class TagsRepository : Repository<Tag>
    {
        public TagsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Set<Tag>()
                .Include(x => x.Posts)
                .ToArrayAsync();
        }
    }
}

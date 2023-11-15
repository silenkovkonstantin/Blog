using Blog.Data.Models.Db;

namespace Blog.Data.Repository
{
    public class TagsRepository : Repository<Tag>
    {
        public TagsRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}

using Blog.Configs;
using Blog.Data.Models.Db;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blog.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        //public DbSet<User> Users { get; set; }
        [Comment("Статьи")]
        public DbSet<Post> UserPosts { get; set; }
        [Comment("Комментарии")]
        public DbSet<Comment> UserComments { get; set; }
        [Comment("Тэги")]
        public DbSet<Tag> PostTags { get; set; }

        //public DbSet<Role> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new CommentConfuiguration());
            builder.ApplyConfiguration(new TagConfuiguration());
            //builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}

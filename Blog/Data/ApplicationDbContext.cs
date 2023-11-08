using Blog.Configs;
using Blog.Models.Db;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Post> UserPosts { get; set; }

        public DbSet<Comment> UserComments { get; set; }

        public DbSet<Tag> PostTags { get; set; }

        public DbSet<Role> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new CommentConfuiguration());
            builder.ApplyConfiguration(new TagConfuiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}

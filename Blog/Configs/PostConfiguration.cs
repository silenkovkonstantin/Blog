using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Blog.Models.Db;

namespace Blog.Configs
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("UserPosts").HasKey(p => p.Id);
            //builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}

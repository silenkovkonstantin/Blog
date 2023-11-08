using Blog.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Configs
{
    public class TagConfuiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("PostTags").HasKey(p => p.Id);
            //builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}

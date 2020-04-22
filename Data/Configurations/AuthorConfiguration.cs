using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Models;

namespace MyBlog.Data.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Permalink)
                .IsRequired()
                .HasMaxLength(500);
            
            //builder.HasMany(x => x.Posts);
        }
    }
}
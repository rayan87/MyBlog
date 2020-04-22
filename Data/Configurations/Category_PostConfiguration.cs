using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Models;

namespace MyBlog.Data.Configurations
{
    public class CategoryPostConfiguration : IEntityTypeConfiguration<CategoryPost>
    {
        public void Configure(EntityTypeBuilder<CategoryPost> builder)
        {
            builder.HasKey(x => new { x.CategoryId, x.PostId });
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.Models
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "Author", NormalizedName = "AUTHOR" }
            );
        }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.PhotoUrl)
                .HasMaxLength(500);
        }
    }

    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.PhotoUrl)
                .HasMaxLength(500);
                
            builder.Property(x => x.Permalink)
                .IsRequired()
                .HasMaxLength(500);
        }
    }

    public class CategoryPostConfiguration : IEntityTypeConfiguration<CategoryPost>
    {
        public void Configure(EntityTypeBuilder<CategoryPost> builder)
        {
            builder.HasKey(x => new { x.CategoryId, x.PostId });
        }
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(500);

            
            //builder.HasMany(x => x.Posts);
        }
    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Permalink)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            //builder.HasMany(x => x.Categories);
            //builder.HasOne(x => x.Author);
        }
    }
}
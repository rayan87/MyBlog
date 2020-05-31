using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Models;
using MyBlog.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyBlog.Data
{
    public class MyBlogContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=MyBlog.db");
            optionsBuilder.UseSqlite("Data Source=MyBlog.db");
        }

        public DbSet<AdminUser> AdminUsers {get;set;}

        public DbSet<Post> Posts {get;set;}

        public DbSet<Category> Categories {get;set;}

        public DbSet<Author> Authors {get;set;}

        public DbSet<CategoryPost> Categories_Posts {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder
                .ApplyConfiguration(new PostConfiguration())
                .ApplyConfiguration(new CategoryConfiguration())
                .ApplyConfiguration(new AuthorConfiguration())
                .ApplyConfiguration(new CategoryPostConfiguration());
        }
    }
}
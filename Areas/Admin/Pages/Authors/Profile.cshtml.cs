using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Authors
{
    public class ProfileModel : AuthorViewModel
    {
        private readonly MyBlogContext _dbContext;

        public ProfileModel(MyBlogContext dbContext) => _dbContext = dbContext;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var author = await _dbContext.Authors
                .AsNoTracking()
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
            
            if (author == null)
                return NotFound();

            PopulateModel(author);

            RecentPosts = await _dbContext.Posts
                .AsNoTracking()
                .Where(x => x.AuthorId == id)
                .OrderByDescending(x => x.Id)
                .Take(3)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);
                
            if (author == null)
                return BadRequest();

            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();
            
            this.InformUser(FormResult.Deleted, 
                $"{author.FirstName} {author.LastName}",
                "author");

            return RedirectToPage("Index");
        }

        public List<Post> RecentPosts
        {
            get;
            private set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Posts
{
    public class NewModel : PageModel
    {
        private readonly MyBlogContext _dbContext;

        public NewModel(MyBlogContext dbContext) => _dbContext = dbContext;

        public async Task OnGetAsync()
        {
            await prepareModelSelectLists();
        }

        public async Task<IActionResult> OnPostAsync(string redirectTo)
        {
            if (!ModelState.IsValid)
            {
                await prepareModelSelectLists();
                return Page();
            }
                
            var entity = Post.ToEntity();
            entity.CreationDate = DateTime.UtcNow;
            entity.LastUpdateDate = DateTime.UtcNow;
            
            await _dbContext.Posts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Added, Post.Title, "post");
            if (redirectTo == "Edit")
                return RedirectToPage("Edit", new { id = entity.Id });
            else
                return RedirectToPage("Index");
        }

        private async Task prepareModelSelectLists()
        {
            Post = new PostViewModel();

            //Categories List
            Post.CategoriesSelectList = await _dbContext.Categories
                    .AsNoTracking()
                    .Select(x => new SelectListItem()
                        { Value = x.Id.ToString(), Text = x.Name }
                    )
                    .ToListAsync();

            //Authors list
            Post.AuthorsSelectList = await _dbContext.Authors
                    .AsNoTracking()
                    .Select(x => 
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.Id.ToString()
                        }
                    )
                    .ToListAsync();
        }

        [BindProperty]
        public PostViewModel Post
        {
            get;set;
        }

    }
}

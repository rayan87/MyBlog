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
    public class EditModel : PageModel
    {
        private readonly MyBlogContext _dbContext;

        public EditModel(MyBlogContext dbContext) => _dbContext = dbContext;

        public async Task<IActionResult> OnGetAsync()
        {
            var entity = await _dbContext.Posts
                .Include(x => x.CategoryPosts)
                .AsNoTracking()
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();

            if (entity == null)
                return NotFound();
            
            await prepareModelForEdit(entity);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string redirectTo)
        {
            if (!ModelState.IsValid)
            {
                await prepareModelSelectLists();
                return Page();
            }

             var entity = await _dbContext.Posts
                .Include(x => x.CategoryPosts)
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();

            if (entity == null)
                return NotFound();
            
            //Delete post categories and insert them again
            _dbContext.Categories_Posts.RemoveRange(entity.CategoryPosts);

            //Update entity with entries in View Model.
            Post.UpdateEntity(entity);

            entity.LastUpdateDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Updated, Post.Title, "post");

            if (redirectTo == "Edit")
                return RedirectToPage("Edit", new { id = entity.Id });
            else
                return RedirectToPage("Index");
        }

        private async Task prepareModelSelectLists()
        {
            //Categories List
            Post.CategoriesSelectList = await _dbContext.Categories
                    .AsNoTracking()
                    .Select(x => new SelectListItem() { 
                        Value = x.Id.ToString(), 
                        Text = x.Name,
                        Selected = Post.SelectedCategories.Contains(x.Id)
                    }).ToListAsync();

            //Authors list
            Post.AuthorsSelectList = await _dbContext.Authors
                    .AsNoTracking()
                    .Select(x =>  new SelectListItem() {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.Id.ToString()
                    }).ToListAsync();
        }

        private async Task prepareModelForEdit(Post post)
        {
            Post = new PostViewModel()
            {
                Title = post.Title,
                Description = post.Description,
                Excerpt = post.Excerpt,
                Tags = post.Tags,
                AuthorId = post.AuthorId,
                Permalink = post.Permalink,
                CreationDate = post.CreationDate,
                LastUpdateDate = post.LastUpdateDate,
                SelectedCategories = post.CategoryPosts.Select(x => x.CategoryId).ToArray()
            };
            
            await prepareModelSelectLists();
        }

        [BindProperty(SupportsGet=true)]
        public int Id {get;set;}

        [BindProperty]
        public PostViewModel Post {get;set;}
    }
}

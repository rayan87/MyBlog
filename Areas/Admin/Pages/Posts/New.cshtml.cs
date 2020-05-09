using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Admin.Services;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Posts
{
    public class NewModel : PageModel
    {
        private readonly MyBlogContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IUploadManager _uploadManager;
        
        public NewModel(MyBlogContext dbContext, 
            IWebHostEnvironment environment,
            IUploadManager uploadManager)
        { 
            _dbContext = dbContext;
            _environment = environment;
            _uploadManager = uploadManager;
        }

        public async Task OnGetAsync()
        {
            await prepareModel();
        }

        public async Task<IActionResult> OnPostAsync(string redirectTo)
        {
            if (!ModelState.IsValid)
            {
                await prepareModel();
                return Page();
            }
            
            var entity = Post.ToEntity();
            entity.CreationDate = DateTime.UtcNow;
            entity.LastUpdateDate = DateTime.UtcNow;
            // if (Post.ImageFile.Length > 0)
            //     entity.ImageUrl = _uploadManager.SaveFileAsync(
            //         Post.ImageFile.OpenReadStream(),
            //         _environment.ContentRootPath)
            
            await _dbContext.Posts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Added, Post.Title, "post");
            if (redirectTo == "Edit")
                return RedirectToPage("Edit", new { id = entity.Id });
            else
                return RedirectToPage("Index");
        }

        public async Task<JsonResult> OnPostCheckPermalink(string permalink)
        {
            bool postExists = await _dbContext.Posts
                .AsNoTracking()
                .AnyAsync(x => x.Permalink == permalink);
            
            return new JsonResult(!postExists);
        }

        private async Task prepareModel()
        {
            Post sourcePost = null;
            if (SourceId.HasValue)
                sourcePost = await _dbContext.Posts
                    .Include(x => x.CategoryPosts)
                    .AsNoTracking()
                    .Where(x => x.Id == SourceId.Value)
                    .SingleOrDefaultAsync();
            
            if (sourcePost != null)
            {
                SourceTitle = sourcePost.Title;
                if (Post == null)
                {
                    Post = new PostViewModel()
                    {
                        AuthorId = sourcePost.AuthorId,
                        Description = sourcePost.Description,
                        Excerpt = sourcePost.Excerpt,
                        Permalink = sourcePost.Permalink,
                        Tags = sourcePost.Tags,
                        Title = sourcePost.Title,
                        SelectedCategories = sourcePost.CategoryPosts.Select(y => y.CategoryId).ToArray()
                    };
                }
            }
            
            if (Post == null)
                Post = new PostViewModel();

            await prepareModelSelectLists();
        }

        //Make sure you this from prepareModel()
        private async Task prepareModelSelectLists()
        {
            //Categories List
            Post.CategoriesSelectList = await _dbContext.Categories
                    .AsNoTracking()
                    .Select(x => new SelectListItem() { 
                        Value = x.Id.ToString(), 
                        Text = x.Name,
                        Selected = Post.SelectedCategories != null && Post.SelectedCategories.Contains(x.Id)
                    }).ToListAsync();

            //Authors list
            Post.AuthorsSelectList = await _dbContext.Authors
                    .AsNoTracking()
                    .Select(x => 
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.Id.ToString()
                        }
                    ).ToListAsync();
        }

        [BindProperty(SupportsGet=true)]
        public int? SourceId {get;set;}

        public string SourceTitle {get;set;}

        [BindProperty]
        public PostViewModel Post
        {
            get;set;
        }


    }
}

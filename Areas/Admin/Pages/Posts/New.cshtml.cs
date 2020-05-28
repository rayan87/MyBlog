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
    public class NewModel : PostViewModel
    {
        private readonly MyBlogContext _dbContext;
        private readonly IUploadManager _uploadManager;
        
        public NewModel(MyBlogContext dbContext, 
            IUploadManager uploadManager)
        { 
            _dbContext = dbContext;
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
            
            var entity = ToEntity();
            entity.CreationDate = DateTime.UtcNow;
            entity.LastUpdateDate = DateTime.UtcNow;
            entity.ImageUrl = await _uploadManager.SavePostImageAsync(ImageFile);
            
            await _dbContext.Posts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Added, Title, "post");
            if (redirectTo == "Edit")
                return RedirectToPage("Edit", new { id = entity.Id });
            else
                return RedirectToPage("Index");
        }

        public async Task<JsonResult> OnPostCheckPermalinkAsync(string permalink)
        {
            bool postExists = await _dbContext.Posts
                .AsNoTracking()
                .AnyAsync(x => x.Permalink == permalink);
            
            return new JsonResult(!postExists);
        }

        private async Task prepareModel()
        {
            var categories = await _dbContext.Categories.AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
            var authors = await _dbContext.Authors.AsNoTracking()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();

            if (SourceId.HasValue)
            {
                var sourcePost = await _dbContext.Posts
                        .Include(x => x.CategoryPosts)
                        .AsNoTracking()
                        .Where(x => x.Id == SourceId.Value)
                        .SingleOrDefaultAsync();
                if (sourcePost != null)
                {
                    SourceTitle = sourcePost.Title;
                    PopulateModel(sourcePost);
                }    
            }
            
            PopulateModelSelectLists(categories, authors);
        }

        [BindProperty(SupportsGet=true)]
        public int? SourceId {get;set;}

        public string SourceTitle {get;set;}

    }
}

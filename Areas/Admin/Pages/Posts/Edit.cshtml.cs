using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
    public class EditModel : PostViewModel
    {
        private readonly MyBlogContext _dbContext;
        private readonly IUploadManager _uploadManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(MyBlogContext dbContext, 
            IUploadManager uploadManager, 
            UserManager<ApplicationUser> userManager) 
        { 
            _dbContext = dbContext;
            _uploadManager = uploadManager;
            _userManager = userManager; 
        }

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
            UpdateEntity(entity);
            
            entity.LastUpdateDate = DateTime.UtcNow;

            //Delete old image
            if (ImageFile != null && ImageFile.Length > 0)
            {
                _uploadManager.RemoveFile(entity.ImageUrl);
                entity.ImageUrl = await _uploadManager.SavePostImageAsync(ImageFile);
            }

            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Updated, Title, "posts");

            if (redirectTo == "Edit")
                return RedirectToPage("Edit", new { id = entity.Id });
            else
                return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var entity = await _dbContext.Posts.FindAsync(Id);
            if (entity == null)
                return NotFound();

            _uploadManager.RemoveFile(entity.ImageUrl);
            _dbContext.Posts.Remove(entity);
            await _dbContext.SaveChangesAsync();
            this.InformUser(FormResult.Deleted, entity.Title, "post");
            return RedirectToPage("Index");
        }

        public async Task<JsonResult> OnPostDeleteImageAsync()
        {
            var entity = await _dbContext.Posts.FindAsync(Id);
            if (entity == null)
                return new JsonResult(false);

            if (!_uploadManager.RemoveFile(entity.ImageUrl))
                return new JsonResult(false);

            entity.ImageUrl = null;
            await _dbContext.SaveChangesAsync();
            return new JsonResult(true);
        }

        private async Task prepareModelForEdit(Post post)
        {
            PopulateModel(post);
            await prepareModelSelectLists();
        }

        private async Task prepareModelSelectLists()
        {
            var categories = await _dbContext.Categories
                    .AsNoTracking()
                    .ToListAsync();

            //Authors list
            var authors = await _dbContext.Authors
                    .AsNoTracking()
                    .ToListAsync();

            PopulateModelSelectLists(categories, authors);
            setupViewPrivilages(authors);
        }

        
        private void setupViewPrivilages(ICollection<Author> authors)
        {
            if (User.IsInRole("Admin"))
            {
                CanSelectAuthor = true;
            }
            else if (User.IsInRole("Author"))
            {
                string userId = _userManager.GetUserId(User);
                AuthorName = authors.Where(x => x.UserId == userId)
                    .Select(x => x.FirstName + " " + x.LastName)
                    .FirstOrDefault();
            }
        }

    }
}

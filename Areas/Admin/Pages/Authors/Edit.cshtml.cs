using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Admin.Services;
using MyBlog.Data;

namespace MyBlog.Admin.Pages.Authors
{
    [Authorize(Roles = "Admin")]
    public class EditModel : AuthorViewModel
    {
        private readonly MyBlogContext _dbContext;
        private readonly IUploadManager _uploadManager;

        public EditModel(MyBlogContext dbContext, 
            IUploadManager uploadManager)
        { 
            _dbContext = dbContext;
            _uploadManager = uploadManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var author = await _dbContext.Authors.FindAsync(Id);
            if (author == null)
                return NotFound();

            PopulateModel(author);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var entity = await _dbContext.Authors.FindAsync(Id);
            if (entity == null)
                return BadRequest();
            
            entity.FirstName = FirstName;
            entity.LastName = LastName;
            entity.JobTitle = JobTitle;
            entity.FullBio = FullBio;
            entity.ShortBio = ShortBio;
            entity.Permalink = Permalink;
            
            
            if (PhotoFile != null && PhotoFile.Length > 0)
                entity.PhotoUrl = await _uploadManager.SaveAuthorImageAsync(PhotoFile);
            
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Updated, $"{entity.FirstName} {entity.LastName}", "author");
            return RedirectToPage("Index");
        }

        public async Task<JsonResult> OnPostCheckPermalinkValidityAsync()
        {
            bool permalinkExists = await _dbContext.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Id != Id && x.Permalink == Permalink);
            
            var isValid = !permalinkExists;
            return new JsonResult(isValid);
        }
    }
}

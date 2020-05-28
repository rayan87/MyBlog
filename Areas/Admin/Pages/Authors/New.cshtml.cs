using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Admin.Services;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Authors
{
    public class NewModel : AuthorViewModel
    {
        private readonly MyBlogContext _dbContext;
        private readonly IUploadManager _uploadManager;
        
        public NewModel(MyBlogContext dbContext, IUploadManager uploadManager)
        {
            _dbContext = dbContext;
            _uploadManager = uploadManager;
        }
        
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
                
            var author = new Author()
            {
                FirstName = FirstName,
                LastName = LastName,
                ShortBio = ShortBio,
                FullBio = FullBio,
                JobTitle = JobTitle,
                Permalink = Permalink,
                ImageUrl = await _uploadManager.SaveAuthorImageAsync(PhotoFile)
            };

            await _dbContext.Authors.AddAsync(author);    
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Added, $"{author.FirstName} {author.LastName}", "author");
            return RedirectToPage("Index");
        }

        public async Task<JsonResult> OnPostCheckPermalinkValidityAsync()
        {
            bool permalinkExists = await _dbContext.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Permalink == Permalink);
            
            var isValid = !permalinkExists;
            return new JsonResult(isValid);
        }
    }
}

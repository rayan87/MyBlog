using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Admin.Services;
using MyBlog.Data;

namespace MyBlog.Admin.Pages.Authors
{
    public class EditModel : PageModel
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

            Author = new AuthorViewModel() 
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                FullBio = author.FullBio,
                JobTitle = author.JobTitle,
                Permalink = author.Permalink,
                ShortBio = author.ShortBio,
                PhotoUrl = author.ImageUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var entity = await _dbContext.Authors.FindAsync(Id);
            if (entity == null)
                return BadRequest();

            entity.FirstName = Author.FirstName;
            entity.LastName = Author.LastName;
            entity.JobTitle = Author.JobTitle;
            entity.FullBio = Author.FullBio;
            entity.ShortBio = Author.ShortBio;
            entity.Permalink = Author.Permalink;
            
            if (Author.PhotoFile != null && Author.PhotoFile.Length > 0)
                entity.ImageUrl = await _uploadManager.SaveAuthorImageAsync(Author.PhotoFile);
            
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Updated, $"{entity.FirstName} {entity.LastName}", "author");
            return RedirectToPage("Index");
        }

        public async Task<JsonResult> OnPostCheckPermalinkValidityAsync(int id, string permalink)
        {
            bool permalinkExists = await _dbContext.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Id != Id && x.Permalink == Author.Permalink);
            
            var isValid = !permalinkExists;
            return new JsonResult(isValid);
        }

        [BindProperty(SupportsGet=true)]
        public int Id {get;set;}

        [BindProperty]
        public AuthorViewModel Author
        {
            get;
            set;
        }
    }
}

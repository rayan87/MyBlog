using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Areas.Admin.Models;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Authors
{
    public class NewModel : PageModel
    {
        private readonly MyBlogContext _dbContext;
        public NewModel(MyBlogContext dbContext) => _dbContext = dbContext;
        
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
                
            var author = new Author()
            {
                FirstName = Author.FirstName,
                LastName = Author.LastName,
                ShortBio = Author.ShortBio,
                FullBio = Author.FullBio,
                JobTitle = Author.JobTitle,
                Permalink = Author.Permalink
            };

            await _dbContext.Authors.AddAsync(author);    
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        [BindProperty]
        public AuthorViewModel Author
        {
            get;
            set;
        }
    }
}

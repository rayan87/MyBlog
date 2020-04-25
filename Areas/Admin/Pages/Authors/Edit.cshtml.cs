using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Areas.Admin.Models;
using MyBlog.Data;

namespace MyBlog.Admin.Pages.Authors
{
    public class EditModel : PageModel
    {
        private readonly MyBlogContext _dbContext;

        public EditModel(MyBlogContext dbContext) => _dbContext = dbContext;

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
                ShortBio = author.ShortBio
            };

            return Page();
        }

        // public async Task<IActionResult> OnPostAsync()
        // {

        // }

        [BindProperty(SupportsGet=true)]
        public int Id {get;set;}

        [BindProperty]
        public AuthorViewModel Author
        {
            get;
            private set;
        }
    }
}

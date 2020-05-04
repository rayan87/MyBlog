using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Authors
{
    public class DeleteModel : PageModel
    {
        private readonly MyBlogContext _dbContext;

        public DeleteModel(MyBlogContext dbContext) => _dbContext = dbContext;

        public async Task<IActionResult> OnGetAsync()
        {
            Author = await _dbContext.Authors
                .Include(x => x.Posts)
                    .ThenInclude(x => x.CategoryPosts)
                        .ThenInclude(x => x.Category)
                .AsNoTracking()
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();

            if (Author == null)
                return NotFound();

            return Page();
        }

        // public async Task<IActionResult> OnPostAsync()
        // {
            
        // }

        [BindProperty(SupportsGet=true)]
        public int Id {get;set;}
        
        public Author Author { get; private set; }
    }
}

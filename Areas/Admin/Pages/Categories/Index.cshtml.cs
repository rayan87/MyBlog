using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Data;

namespace MyBlog.Admin.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly MyBlogContext _dbContext;

        public IndexModel(MyBlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGetAsync()
        {
            Categories = await _dbContext.Categories
                .Include(x => x.CategoryPosts)
                .AsNoTracking()
                .Select(x => new CategoryViewModel() {
                    Id = x.Id,
                    Name = x.Name,
                    Posts = x.CategoryPosts.Count()
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnDeleteAsync(int id)
        {
            _dbContext.Categories.Remove(new Data.Models.Category() { Id = id });
            await _dbContext.SaveChangesAsync();
            return Content("1");
        }

        public List<CategoryViewModel> Categories
        {
            get;
            private set;
        }
    }
}

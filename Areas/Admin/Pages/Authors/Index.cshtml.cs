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
    public class IndexModel : PageModel
    {
        private readonly MyBlogContext _dbContext;
        public IndexModel(MyBlogContext dbContext) => _dbContext = dbContext;

        public async Task OnGetAsync()
        {
            Authors = await _dbContext.Authors
                .Include(x => x.Posts)
                .AsNoTracking()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();
        }

        public List<Author> Authors
        {
            get;
            private set;
        }
    }
}

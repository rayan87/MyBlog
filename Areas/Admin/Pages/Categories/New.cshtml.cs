using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Admin.Models;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Categories
{
    public class NewModel : PageModel
    {
        private readonly MyBlogContext _dbContext;
        public NewModel(MyBlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var category = new Category()
            {
                Name = Category.Name
            };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Added, category.Name, "category");
            return RedirectToPage("Index");
        }

        [BindProperty]
        public CategoryViewModel Category
        {
            get;set;
        }
    }
}

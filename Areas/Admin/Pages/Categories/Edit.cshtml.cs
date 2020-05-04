using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Admin.Models;
using MyBlog.Data;

namespace MyBlog.Admin.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly MyBlogContext _dbContext;

        public EditModel(MyBlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var category = await _dbContext.Categories.FindAsync(Id);
            if (category == null)
                return NotFound();

            Category = new CategoryViewModel() { Name = category.Name };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var category = await _dbContext.Categories.FindAsync(Id);
            if (category == null)
                return BadRequest();

            category.Name = Category.Name;
            await _dbContext.SaveChangesAsync();

            this.InformUser(FormResult.Updated, category.Name, "category");
            return RedirectToPage("Index");
        }

        [BindProperty(SupportsGet=true)]
        public int Id {get;set;}

        [BindProperty]
        public CategoryViewModel Category
        {
            get;
            set;
        }
    }
}

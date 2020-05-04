using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly MyBlogContext _dbContext;

        public IndexModel(MyBlogContext dbContext) => _dbContext = dbContext;

        public async Task OnGetAsync()
        {
            var postsQuery = _dbContext.Posts
                .Include(x => x.Author)
                .Include(x => x.CategoryPosts)
                    .ThenInclude(x => x.Category)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                postsQuery = postsQuery.Where(x => EF.Functions.Like(x.Title, $"%{search}%")
                    || EF.Functions.Like(x.Description, $"%{search}%")
                    || EF.Functions.Like(x.Tags, $"%{search}%"));

            if (authorId.HasValue)
                postsQuery = postsQuery.Where(x => x.AuthorId == authorId.Value);

            if (categoryId.HasValue)
                postsQuery = postsQuery.Where(x => x.CategoryPosts.Any(y => y.CategoryId == categoryId));

            if (Sort.HasValue)
            {
                switch (Sort.Value)
                {
                    case PostSortBy.Alphabatical:
                        postsQuery = postsQuery.OrderBy(x => x.Title);
                        break;
                    case PostSortBy.Newest:
                        postsQuery = postsQuery.OrderByDescending(x => x.CreationDate);
                        break;
                    case PostSortBy.Oldest:
                        postsQuery = postsQuery.OrderBy(x => x.CreationDate);
                        break;
                }
            }
            else
                postsQuery = postsQuery.OrderByDescending(x => x.CreationDate);
            
            Posts = await postsQuery.ToListAsync();

            await prepareSelectListsAsync();
            prepareSubTitle();
        }

        public async Task<IActionResult> OnDeleteAsync(int id)
        {
            _dbContext.Remove(new Post() { Id = id });
            await _dbContext.SaveChangesAsync();
            return Content("1");
        }

        private async Task prepareSelectListsAsync()
        {
            AuthorSelectList = await _dbContext.Authors
                .AsNoTracking()
                .Select(x => new SelectListItem() {
                    Value = x.Id.ToString(),
                    Text = x.FirstName + " " + x.LastName
                })
                .ToListAsync();

            CategorySelectList = await _dbContext.Categories
                .AsNoTracking()
                .Select(x => new SelectListItem() {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        
        private void prepareSubTitle()
        {
            //Author's Posts in Category
            string authorName, categoryName;

            if (authorId.HasValue)
                authorName = AuthorSelectList
                    .Where(x => x.Value == authorId.Value.ToString())
                    .Select(x => x.Text)
                    .FirstOrDefault();
            else
                authorName = "All";
            
            if (categoryId.HasValue)
                categoryName = " in " + CategorySelectList
                    .Where(x => x.Value == categoryId.Value.ToString())
                    .Select(x => x.Text)
                    .FirstOrDefault();
            else
                categoryName = "";

            SubTitle = string.Format("{0} Posts{1}", authorName, categoryName);
        }


        public string SubTitle {get;set;}

        [BindProperty(SupportsGet=true)]
        public string search {get; set;}

        [BindProperty(SupportsGet=true)]
        public int? authorId {get;set;} 

        [BindProperty(SupportsGet=true)]
        public int? categoryId {get;set;}

        public List<SelectListItem> AuthorSelectList {get;set;}

        public List<SelectListItem> CategorySelectList {get;set;}

        public List<Post> Posts
        {
            get;
            private set;
        }

        [BindProperty(SupportsGet=true)]
        public PostSortBy? Sort {get;set;}
    }
}

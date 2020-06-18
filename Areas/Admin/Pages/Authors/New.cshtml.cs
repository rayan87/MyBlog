using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Models;
using MyBlog.Admin.Services;
using MyBlog.Data;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Authors
{
    [Authorize(Roles = "Admin")]
    public class NewModel : AuthorViewModel
    {
        private readonly MyBlogContext _dbContext;
        private readonly IUploadManager _uploadManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewModel(MyBlogContext dbContext, 
            IUploadManager uploadManager,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _uploadManager = uploadManager;
            _userManager = userManager;
        }
        
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "Password must match confirm password");
                return Page();
            }
                
            //save photo
            var photoUrl = await _uploadManager.SaveAuthorImageAsync(PhotoFile);

            var author = new Author()
            {
                FirstName = FirstName,
                LastName = LastName,
                ShortBio = ShortBio,
                FullBio = FullBio,
                JobTitle = JobTitle,
                Permalink = Permalink,
                PhotoUrl = photoUrl
            };

            //Populate identity info
            //Try to save identity info first
            author.User = new ApplicationUser() 
            {
                UserName = Email,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                PhotoUrl = photoUrl
            };

            await _dbContext.Authors.AddAsync(author);    
            
            var userCreationResult = await _userManager.CreateAsync(author.User, Password);
            
            if (userCreationResult.Succeeded)
            {   
                await _userManager.AddToRoleAsync(author.User, "Author");
                this.InformUser(FormResult.Added, $"{author.FirstName} {author.LastName}", "author");
                return RedirectToPage("Index");    
            }
            else
            {
                foreach (var err in userCreationResult.Errors)
                    ModelState.AddModelError("", err.Description);

                return Page();
            }
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

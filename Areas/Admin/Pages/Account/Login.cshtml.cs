using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AdminUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public LoginModel(SignInManager<AdminUser> signInManager,
            IWebHostEnvironment environment)
        {
            _signInManager = signInManager;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return Page();
            else
                return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            var signInResult = await _signInManager.PasswordSignInAsync(Email, Password, RememberMe, false);
            UserManager<AdminUser> userManager;
            
            if (signInResult.Succeeded)
                //Logged-in successfully
                return RedirectToPage("/Index");
            else if (signInResult.IsNotAllowed)
                //Logged-in but, Email not confirmed
                DisplayNotConfirmedError = true;
            else
                //Invalid login attempt
                ModelState.AddModelError("", "Email or password is invalid.");
            
            return Page();
            
        }

        #region Properties

        public bool DisplayNotConfirmedError {get;private set;}

        [BindProperty] 
        [Required]
        [DataType(DataType.EmailAddress)] 
        [Display(Prompt="Email")]
        public string Email {get;set;}

        [BindProperty] 
        [Required]
        [DataType(DataType.Password)]
        [Display(Prompt="Password")]
        public string Password {get;set;}

        [BindProperty]
        public bool RememberMe {get;set;}

        #endregion
    }
}

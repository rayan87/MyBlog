using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly SignInManager<AdminUser> _signInManager;

        public RegisterModel(UserManager<AdminUser> userManager,
            SignInManager<AdminUser> signInManager)
        {
            _userManager = userManager;    
            _signInManager = signInManager;
        }

        #region Handlers

        public async Task<IActionResult> OnGetAsync()
        {
            //If there are users in the db, just redirect to login.
            bool dbHasUsers = await _userManager.Users.AnyAsync();
            if (dbHasUsers)
                return RedirectToPage("Login");
            
            //Otherwise, user can register.
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "Retyping password must match password.");
                return Page();
            }

            var names = FullName.Split(' ');
            var admin = new AdminUser()
            {
                FirstName = names[0],
                LastName = names.Length > 1 ? names[1] : null,
                Email = Email,
                UserName = Email
            };

            var result = await _userManager.CreateAsync(admin, Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(admin, isPersistent: false);
                return RedirectToPage("/Index");
            }
            else
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);

                return Page();
            }
        }

        #endregion

        #region Properties

        [Required]
        [Display(Prompt = "Full name")]
        [BindProperty]
        public string FullName {get;set;}

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt="Email")]
        [BindProperty]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password", Prompt="Password")]
        [BindProperty]
        public string Password {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Prompt="Retype password")]
        [BindProperty]
        public string ConfirmPassword {get;set;}

        #endregion

    }
}
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
        private readonly UserManager<AdminUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public LoginModel(SignInManager<AdminUser> signInManager,
            UserManager<AdminUser> userManager,
            IWebHostEnvironment environment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
            
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, Password)))
            {
                //Invalid login attempt
                ModelState.AddModelError("", "Email or password is invalid.");
                return Page();
            }
                
            bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                DisplayNotConfirmedError = true;
                return Page();
            }

            await _signInManager.SignInAsync(user, RememberMe);
            return RedirectToPage("/Index");
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

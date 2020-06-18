using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBlog.Admin.Services.Email;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public RegisterModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;    
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        #region Handlers

        public async Task<IActionResult> OnGetAsync()
        {
            //There shouldn't be admins to register
            bool dbHasUsers = (await _userManager.GetUsersInRoleAsync("Admin")).Any();
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
            var user = new ApplicationUser()
            {
                FirstName = names[0],
                LastName = names.Length > 1 ? names[1] : null,
                Email = Email,
                UserName = Email
            };
            
            var result = await _userManager.CreateAsync(user, Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                //try sending registration confirmation email
                bool isEmailSent = await sendVerificationEmail(user);

                if (isEmailSent)
                    return RedirectToPage("RegisterConfirmation");
                else
                    return RedirectToPage("SendCode");
            }
            else
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);

                return Page();
            }
        }

        private async Task<bool> sendVerificationEmail(ApplicationUser user)
        {
            string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string emailConfirmationLink = Url.Page("VerifyAccount", 
                null,
                new { userId = user.Id, code = emailConfirmationToken},
                Request.Scheme);

            string subject = "Verify Your Account";

            StringBuilder emailMessage = new StringBuilder();
            emailMessage.AppendFormat("<p>Dear {0}, </p><br>", user.FirstName);
            emailMessage.AppendFormat("<p>Please confirm your registration as Administrator to My Blog Control Panel by clicking on the link below:</p>");
            emailMessage.AppendFormat("<div style=\"border:1px solid #b2b2b2;background-color:#f2f2f2;padding:5px\"><a href=\"{0}\">{0}</a></div>",
                emailConfirmationLink);

            return await _emailSender.SendAsync(user.Email, subject, emailMessage.ToString());
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
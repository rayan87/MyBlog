using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AdminUser> _userManager;

        public ForgotPasswordModel(UserManager<AdminUser> userManager) 
            => _userManager = userManager;

        public async Task<IActionResult> OnPostAsync()
        {
            //check if email is correct.
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email does not exist.");
                return Page();
            }
                
            //generate reset password token.
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            //send email to the user with the new token.
            
            //the new link goes to ResetPassword page.
            return Page();
        }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "Email")]
        public string Email {get;set;}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Admin.Services.Email;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<AdminUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        } 
            

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

            await sendPasswordResetEmail(user.Id, passwordResetToken, user.Email, user.FirstName);

            return Page();
        }

        private Task sendPasswordResetEmail(string userId, string code, string email, string firstName)
        {
            //send email to the user with the new token.
            var passwordResetLink = Url.Page("ResetPassword", 
                null,
                new { userId, code },
                Request.Scheme);

            var emailMessage = new StringBuilder();

            emailMessage.AppendFormat("<p>Dear {0}</p><br><br>", firstName);
            emailMessage.AppendFormat("<p>You requested to reset your password.</p><br>");
            emailMessage.AppendFormat("<p>Click on the following link to reset your password:</p><br><br>");
            emailMessage.AppendFormat("<div style=\"border:1px solid #eee;background-color:#e2e2e2;padding:2px\"><a href=\"{0}\">{0}</a></div>",
                passwordResetLink);

            //_emailSender.SendAsync()
            return Task.CompletedTask;

        }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "Email")]
        public string Email {get;set;}
    }
}

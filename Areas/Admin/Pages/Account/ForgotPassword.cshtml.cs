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
            
            bool isEmailSent = await sendPasswordResetEmail(user);
            if (isEmailSent)
                return RedirectToPage("ForgotPasswordConfirmation");
            else
            {
                ModelState.AddModelError("", "An error occured while sending email, please try again later.");
                return Page();
            }
        }

        private async Task<bool> sendPasswordResetEmail(AdminUser user)
        {
            //generate reset password token.
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            string subject = "Password Reset Request";

            //send email to the user with the new token.
            var passwordResetLink = Url.Page("ResetPassword", 
                null,
                new { userId = user.Id, code = passwordResetToken },
                Request.Scheme);

            //prepare email message
            var emailMessage = new StringBuilder();

            emailMessage.AppendFormat("<p>Dear {0}</p><br><br>", user.FirstName);
            emailMessage.AppendFormat("<p>You requested to reset your password.</p><br>");
            emailMessage.AppendFormat("<p>Click on the following link to reset your password:</p>");
            emailMessage.AppendFormat("<div style=\"border:1px solid #b2b2b2;background-color:#f2f2f2;padding:5px\"><a href=\"{0}\">{0}</a></div>",
                passwordResetLink);

            return await _emailSender.SendAsync(user.Email, 
                subject,
                emailMessage.ToString());
        }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "Enter your email")]
        public string Email {get;set;}
    }
}

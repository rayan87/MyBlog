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
    public class SendCodeModel : PageModel
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly IEmailSender _emailSender;

        public SendCodeModel(UserManager<AdminUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public void OnGet(string source)
        {
            if (source == "register")
                ModelState.AddModelError("", "Cannot send registration confirmation email.");
        }

        public async Task<ActionResult> OnPostAsync()
        {
            //Check for user by email.
            var user = await _userManager.FindByEmailAsync(Email);

            //If user is not found, just display success message,
            if (user == null)
                return RedirectToPage("SendCodeConfirmation");

            
            bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                //If user is found but not confirmed, send email    
                if (await sendVerificationEmail(user))
                    return RedirectToPage("SendCodeConfirmation");
                else
                {
                    ModelState.AddModelError("", "Error sending verification email.");
                    return Page();
                }
            }
            else
            {
                //If user is found and confirmed
                //display warning message that their account is already confirmed.
                EmailAlreadyConfirmed = true;
                return Page();
            }
        }

        private async Task<bool> sendVerificationEmail(AdminUser user)
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

        public bool EmailAlreadyConfirmed {get;set;}

        public bool DisplayRegistrationError {get; private set;}
        
        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "Enter your email")]
        public string Email {get;set;}
    }
}

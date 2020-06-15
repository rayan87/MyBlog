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
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AdminUser> _userManager;

        public ResetPasswordModel(UserManager<AdminUser> userManager)
            => _userManager = userManager;

        public async Task OnGetAsync()
        {
            //Check if paramaters were provided
            if (string.IsNullOrWhiteSpace(UserId) ||
                string.IsNullOrWhiteSpace(Code))
                return;

            //Check if user exists
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return;
            
            //If code reached here then,
            //Everything is OK, show the form.
            ShowForm = true;
        }

        public async Task OnPostAsync()
        {
            ShowForm = true;

            //check if data is valid
            if (!ModelState.IsValid)
                return;

            //check if password and confirm password match
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "Retype new password must match new password.");
                return;
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                //User does not exist    
                //Bad request ,don't show form
                ShowForm = false;
                return;
            }
                
            
            //try to reset password
            var result = await _userManager.ResetPasswordAsync(user, Code, Password);
            if (result.Succeeded)
            {
                ShowForm = false;
                Succeeded = true;
            }
            else
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }
        }

        public bool ShowForm {get;private set;}

        public bool Succeeded {get;private set;}

        public IEnumerable<string> AdditionalErrors {get;private set;}

        [BindProperty(SupportsGet = true)]
        public string UserId {get;set;}

        [BindProperty(SupportsGet = true)]
        public string Code {get;set;}

        [BindProperty, Required, DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Enter your new password")]
        public string Password {get;set;}

        [BindProperty, Required, DataType(DataType.Password)]
        [Display(Name = "Confirm Password", Prompt = "Retype your new password")]
        public string ConfirmPassword {get;set;}
    }
}

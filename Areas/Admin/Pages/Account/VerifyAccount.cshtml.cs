using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Pages.Account
{
    public class VerifyAccountModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VerifyAccountModel(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public async Task OnGetAsync(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                Succeeded = false;
                return;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Succeeded = false;
                return;
            }

            var confirmationResult = await _userManager.ConfirmEmailAsync(user, code);
            if (confirmationResult.Succeeded)
            {
                Succeeded = true;
                return;
            }
            else
            {
                //Error verifying the account. (specify errors)
                Succeeded = false;
                AdditionalErrors = confirmationResult.Errors.Select(x => x.Description);
            }
        }

        public bool Succeeded {get;private set;}

        public IEnumerable<string> AdditionalErrors {get;set;}


    }
}

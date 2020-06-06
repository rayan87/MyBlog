using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyBlog.Admin.Pages.Account
{
    public class ResendRegistrationCodeModel : PageModel
    {
        public void OnGet(string source)
        {
            if (source == "register")
                ModelState.AddModelError("", "Cannot send registration confirmation email.");
        }


        public bool DisplayRegistrationError {get; private set;}
        
        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "Enter your email")]
        public string Email {get;set;}
    }
}

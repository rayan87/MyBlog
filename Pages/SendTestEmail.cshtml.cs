using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Admin.Services.Email;

namespace MyBlog.Pages
{
    public class SendTestEmailModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        public SendTestEmailModel(IEmailSender emailSender) => _emailSender = emailSender;

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            EmailSent = await _emailSender.SendAsync(ToEmail, Subject, Body);
        }

        public bool EmailSent {get;set;}

        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string ToEmail {get;set;}

        [BindProperty]
        public string Subject {get;set;}

        [BindProperty]
        public string Body {get;set;}
    }
}

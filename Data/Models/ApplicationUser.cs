using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MyBlog.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string PhotoUrl {get;set;}
    }
}
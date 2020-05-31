using Microsoft.AspNetCore.Identity;

namespace MyBlog.Data.Models
{
    public class AdminUser : IdentityUser
    {
        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string PhotoUrl {get;set;}
    }
}
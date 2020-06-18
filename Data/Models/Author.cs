using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyBlog.Data.Models
{
    public class Author
    {
        public int Id {get;set;}

        public string UserId {get;set;}
        
        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string PhotoUrl {get;set;}

        public string JobTitle {get;set;}

        public string ShortBio {get;set;}

        public string FullBio {get;set;}

        public string Permalink {get;set;}

        [ForeignKey("UserId")]
        public ApplicationUser User {get;set;}

        public ICollection<Post> Posts { get; set; }
    }
}
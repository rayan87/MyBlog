using System.Collections.Generic;

namespace MyBlog.Data.Models
{
    public class Author
    {
        public int Id {get;set;}

        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string ShortBio {get;set;}

        public string FullBio {get;set;}

        public string Permalink {get;set;}

        public ICollection<Post> Posts { get; set; }
    }
}
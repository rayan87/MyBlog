using System;
using System.Collections.Generic;

namespace MyBlog.Data.Models
{
    public class Post
    {
        public int Id {get;set;}

        public int AuthorId {get;set;}

        public string Title {get;set;}

        public string Description {get;set;}

        public string Excerpt {get;set;}

        public string Tags {get;set;}

        public string Permalink {get;set;}

        public DateTime CreationDate {get;set;}

        public DateTime LastUpdateDate {get;set;}

        public Author Author {get;set;}

        public ICollection<CategoryPost> CategoryPosts {get;set;}
    }
}
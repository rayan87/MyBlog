using System.Collections.Generic;

namespace MyBlog.Data.Models
{
    public class CategoryPost
    {
        public int CategoryId {get;set;}

        public int PostId {get;set;}

        public Post Post {get;set;}

        public Category Category {get;set;}
    }
}
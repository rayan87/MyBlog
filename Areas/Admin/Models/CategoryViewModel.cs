using System.ComponentModel.DataAnnotations;

namespace MyBlog.Admin.Models
{
    public class CategoryViewModel
    {
        public int Id {get;set;}

        [Required]
        public string Name {get;set;}

        public int Posts {get;set;}

    }
}
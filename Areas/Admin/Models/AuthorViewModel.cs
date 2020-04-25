using System.ComponentModel.DataAnnotations;

namespace MyBlog.Areas.Admin.Models
{
    public class AuthorViewModel
    {
        

        #region Model Properties

        [Required, MinLength(3), MaxLength(20)]
        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string JobTitle {get;set;}

        public string ShortBio {get;set;}

        public string FullBio {get;set;}

        [Required]
        public string Permalink {get;set;}

        #endregion    
    }
}
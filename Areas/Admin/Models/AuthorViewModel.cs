using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Admin.Models
{
    public class AuthorViewModel
    {
        [Display(Name = "First Name")]
        [Required, MinLength(3), MaxLength(20)]
        public string FirstName {get;set;}

        [Display(Name = "Last Name")]
        public string LastName {get;set;}

        [Display(Name = "Job Title")]
        public string JobTitle {get;set;}

        [Display(Name = "Short Bio")]
        public string ShortBio {get;set;}

        [Display(Name = "Full Bio")]
        public string FullBio {get;set;}

        [Required]
        // [PageRemote(PageHandler="CheckPermalinkValidity",
        // ErrorMessage="Permalink already exists",
        // AdditionalFields="__RequestVerificationToken,Id",
        // HttpMethod="post")]
        public string Permalink {get;set;}
    }
}
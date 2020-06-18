using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.Models;

namespace MyBlog.Admin.Models
{
    [BindProperties]
    public class AuthorViewModel : PageModel
    {
        [BindProperty(SupportsGet=true)]
        public int? Id {get;set;}

        [Display(Name = "First Name")]
        [Required, MinLength(3), MaxLength(20)]
        public string FirstName {get;set;}

        [Display(Name = "Last Name")]
        public string LastName {get;set;}

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {get;set;}

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password {get;set;}


        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}

        [Display(Name = "Job Title")]
        public string JobTitle {get;set;}

        [Display(Name = "Short Bio")]
        public string ShortBio {get;set;}

        [Display(Name = "Full Bio")]
        public string FullBio {get;set;}

        public IFormFile PhotoFile {get;set;}

        public string PhotoUrl {get;set;}

        [Required]
        [PageRemote(PageHandler="CheckPermalinkValidity",
        ErrorMessage="Permalink already exists",
        AdditionalFields="__RequestVerificationToken,Id",
        HttpMethod="post")]
        public string Permalink {get;set;}

        protected void PopulateModel(Author entity)
        {
            Id = entity.Id;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            JobTitle = entity.JobTitle;
            ShortBio = entity.ShortBio;
            FullBio = entity.FullBio;
            PhotoUrl = entity.PhotoUrl;
            Permalink = entity.Permalink;
        }

    }
}
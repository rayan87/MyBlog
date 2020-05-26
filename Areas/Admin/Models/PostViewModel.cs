using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.Data.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MyBlog.Admin.Models
{
    public class PostViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Title", Description="Post Title"), Required]
        public string Title {get;set;}

        [Display(Name = "Description", Description="Post Description")]
        public string Description {get;set;}

        [Required(ErrorMessage="You must select at least 1 category")]
        public int[] SelectedCategories {get;set;}

        public string Excerpt {get;set;}

        public string Tags {get;set;}

        //Multi-select
        public List<SelectListItem> CategoriesSelectList { get; set; }

        [Display(Name="Author"), Required]
        public int AuthorId {get;set;}

        public List<SelectListItem> AuthorsSelectList {get;set;}

        [Display(Name = "Permalink"), Required, PageRemote(
            ErrorMessage ="Permalink already exists", 
            AdditionalFields = "__RequestVerificationToken", 
            HttpMethod ="post",  
            PageHandler ="CheckPermalink"
        )]
        public string Permalink {get;set;}

        public IFormFile ImageFile {get;set;}

        public string ImageUrl {get;set;}

        [Display(Name = "Last Update Date")]
        public DateTime? LastUpdateDate {get;set;}

        [Display(Name = "Posted on")]
        public DateTime? CreationDate {get;set;}

        public Post ToEntity()
        {
            var entity = new Post();
            UpdateEntity(entity);
            return entity;
        }

        public void UpdateEntity(Post entity)
        {
            entity.Title = this.Title;
            entity.Description = this.Description;
            entity.AuthorId = this.AuthorId;
            entity.Permalink = this.Permalink;
            entity.CategoryPosts = new List<CategoryPost>();
            entity.Tags = this.Tags;
            entity.Excerpt = this.Excerpt;

            if (SelectedCategories != null)
                foreach (var catId in SelectedCategories)
                    entity.CategoryPosts.Add(new CategoryPost() { CategoryId = catId });
        }


    }
}
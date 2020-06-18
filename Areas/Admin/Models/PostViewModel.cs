using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.Data.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyBlog.Admin.Models
{
    [BindProperties]
    public class PostViewModel : PageModel
    {
        [BindProperty(SupportsGet=true)]
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
        public IEnumerable<SelectListItem> CategoriesSelectList { get; set; }

        [Display(Name="Author"), Required]
        public int AuthorId {get;set;}

        public string AuthorName {get;set;}

        public IEnumerable<SelectListItem> AuthorsSelectList {get;set;}

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

        protected void PopulateModel(Post entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            Excerpt = entity.Excerpt;
            Tags = entity.Tags;
            AuthorId = entity.AuthorId;
            Permalink = entity.Permalink;
            CreationDate = entity.CreationDate;
            LastUpdateDate = entity.LastUpdateDate;
            ImageUrl = entity.ImageUrl;
            SelectedCategories = entity.CategoryPosts.Select(x => x.CategoryId).ToArray();

        }

        protected void PopulateModel(Post entity, List<Category> categories, List<Author> authors)
        {
            PopulateModel(entity);
            PopulateModelSelectLists(categories, authors);
        }

        protected void PopulateModelSelectLists(List<Category> categories, List<Author> authors)
        {
            //Categories List
            CategoriesSelectList = categories
                    .Select(x => new SelectListItem() { 
                        Value = x.Id.ToString(), 
                        Text = x.Name,
                        Selected = SelectedCategories != null && SelectedCategories.Contains(x.Id)
                    });

            //Authors list
            AuthorsSelectList = authors
                    .Select(x =>  new SelectListItem() {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.Id.ToString(),
                        Selected = x.Id == AuthorId
                    });
        }
        
        #region Privilige Properties

        public bool CanSelectAuthor {get;set;}

        #endregion
        

    }
}
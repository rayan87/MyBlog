using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data;

namespace MyBlog.Pages
{
    public class WelcomeModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private MyBlogContext _dbContext;

        public WelcomeModel(IWebHostEnvironment environment,
            MyBlogContext dbContext)
        { 
            _environment = environment;
            _dbContext = dbContext;    
        }
        
        public void OnGet()
        {
            ApplicationName = _environment.ApplicationName;
            ContentRootPath = _environment.ContentRootPath;
            EnvironmentName = _environment.EnvironmentName;
            WebRootPath = _environment.WebRootPath;

        }

        public string ApplicationName {get;private set;}
        public string ContentRootPath { get; private set; }
        public string EnvironmentName { get; private set; }
        public string WebRootPath { get; private set; }
        public string Sql {get;set;}
    }
}

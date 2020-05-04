using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyBlog.Admin.Models
{
    public static class FormResultExtensions
    {
        public static void InformUser(this PageModel page, FormResult result, string itemName, string moduleName)
        {
            page.TempData["Operation"] = result;
            page.TempData["ItemName"] = itemName;
            page.TempData["ModuleName"] = moduleName;
        }
    }

    public enum FormResult
    {
        Added,
        Updated,
        Deleted
    }
}
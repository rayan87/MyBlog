using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyBlog.Admin.Services
{
    public interface IUploadManager
    {
        Task<string> SavePostImageAsync(IFormFile fileUpload);

        Task<string> SaveAuthorImageAsync(IFormFile fileUpload);

        bool RemoveFile(string fileUrl);
    }
}
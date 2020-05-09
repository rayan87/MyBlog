using System.IO;
using System.Threading.Tasks;

namespace MyBlog.Admin.Services
{
    public interface IUploadManager
    {
        Task<string> SaveFileAsync(Stream uploadStream, string folder, string fileName);
    }
}
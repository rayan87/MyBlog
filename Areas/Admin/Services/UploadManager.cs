using System.Threading.Tasks;
using System.IO;

namespace MyBlog.Admin.Services
{
    public class UploadManager : IUploadManager
    {
        private readonly string _contentRootPath;
        

        public UploadManager(string contentRootPath)
        {
            _contentRootPath = contentRootPath;
        }

        

        public async Task<string> SaveFileAsync(Stream uploadStream, string folder, string fileName)
        {
            //Saves file and returns URL
            string filePath = Path.Combine(_contentRootPath, folder, fileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                await uploadStream.CopyToAsync(fileStream);
            
            return filePath.Replace("\\" , "/");
        }        
    }
}
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace MyBlog.Admin.Services
{
    public class UploadManager : IUploadManager
    {
        private readonly string _webRootPath;

        private const string _uploadsFolder = "uploads";
        
        const string _postImagesFolder = "posts";
        const string _authorImagesFolder = "authors";
        
        public UploadManager(IWebHostEnvironment environment)
        {
            _webRootPath = environment.WebRootPath;
        }

        public async Task<string> SavePostImageAsync(IFormFile fileUpload)
        {
            return await saveFileAsync(fileUpload, _postImagesFolder);
        }

        public async Task<string> SaveAuthorImageAsync(IFormFile fileUpload)
        {
            return await saveFileAsync(fileUpload, _authorImagesFolder);
        }

        public bool RemoveFile(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return false;

            string filePath = Path.Combine(_webRootPath, fileUrl.Replace('/', '\\'));
            var file = new FileInfo(filePath);
            if (!file.Exists)
                return false;
            
            file.Delete();
            return true;
        }

        private async Task<string> saveFileAsync(IFormFile fileUpload, string targetFolder)
        {
            if (fileUpload == null || fileUpload.Length == 0)
                return null;

            //Saves file and returns URL
            string fileFolder = Path.Combine(_webRootPath, _uploadsFolder, targetFolder);
            createFolderIfNotExist(fileFolder);

            string filePath = getNewFilePath(fileFolder, fileUpload.FileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                await fileUpload.CopyToAsync(fileStream);
            
            //Returns virtual path of uploaded file.
            return string.Join('/', _uploadsFolder, targetFolder, fileUpload.FileName);
        }

        private void createFolderIfNotExist(string folder)
        {
            var dir = new DirectoryInfo(folder);
            if (!dir.Exists)
                dir.Create();
        }

        private string getNewFilePath(string fileFolder, string fileName)
        {
            string filePath = Path.Combine(fileFolder, fileName);
            
            for (int i = 1; File.Exists(filePath); i++)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = string.Format("{0}_{1}{2}",
                    fileNameWithoutExtension,
                    i,
                    Path.GetExtension(fileName));
                filePath = Path.Combine(fileFolder, fileName);
            }
            
            return filePath;
        }
    }
}
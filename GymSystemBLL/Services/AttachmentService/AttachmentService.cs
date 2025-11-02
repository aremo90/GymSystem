using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long MaxFileSize = 5 * 1024 * 1024; // 5mb

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                // Check extention and file size
                if (folderName is null || file is null || file.Length == 0) return null;
                if (file.Length > MaxFileSize) return null;
                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(ext)) return null;

                // get folderPath
                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                };

                //GUID
                var FileName = Guid.NewGuid().ToString() + ext;

                // FilePath
                var FilePath = Path.Combine(FolderPath, FileName);

                // Stream
                using var FileStream = new FileStream(FilePath, FileMode.Create);
                // CopyTo Stream
                file.CopyTo(FileStream);

                return FileName;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Faild to Upload : {ex}");
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

                var FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images" , folderName , fileName);
                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faild to Delete : {ex}");
                return false;
            }
        }

    }
}

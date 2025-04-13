using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        public readonly List<string> _allowedExtensions = new() { ".png", ".jpg", ".jpeg" };
        public const int _maxAllowedSize = 2_097_152;
        public async Task<string?> Upload(IFormFile file, string folderName)
        {
            //1]Validate for extensions [".png", ".jpg", ".jpeg" ]
            var extension = Path.GetExtension(file.FileName);
            if(!_allowedExtensions.Contains(extension)) 
                return null;
            //2]Validate for Max size[2_097_152; //2MB]
            if(file.Length > _maxAllowedSize)
                return null;
            //3]Get located folder path
            //var folderPath = "C:\\Users\\DELL\\Desktop\\Route.net\\C#\\MVC_GroupOne\\Demo.PL\\wwwroot\\files\\images";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            //4]Set unique file name
            var fileName = $"{Guid.NewGuid()}{extension}";
            //5]Get file path [FolderPath + FileName]
            var filePath = Path.Combine(folderPath, fileName);
            //6]Save file as stream[Data per time]
            using var fileStream = new FileStream(filePath, FileMode.Create);
            //7]Copy file to the stream
            await file.CopyToAsync(fileStream);
            //8]Return file name
            return fileName;
        }
        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

    }
}

using Graduation_Project.Core.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Service
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName , ClaimsPrincipal? user, string? customFileName = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file format. Allowed formats: .jpg, .jpeg, .png, .gif, .bmp, .webp");

            // Validate file size (Max 5MB)
            const long maxFileSize = 5 * 1024 * 1024;
            if (file.Length > maxFileSize)
                throw new ArgumentException("File size must be less than 5MB.");

            // Validate folder name
            if (string.IsNullOrWhiteSpace(folderName))
                throw new ArgumentException("Folder name is required.");

            //if(CustomFileName is null)
            //{
            //    string uniqueFileName = $"{user.FindFirstValue(ClaimTypes.GivenName)!}-{user.FindFirstValue(ClaimTypes.NameIdentifier)!}{extension}";
            //}
            //else
            //{

            //}

            string uniqueFileName = string.IsNullOrWhiteSpace(customFileName)
                ? $"{user.FindFirstValue(ClaimTypes.GivenName)!}-{user.FindFirstValue(ClaimTypes.NameIdentifier)!}{extension}"
                : $"{customFileName}{extension}"
                ;

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}"; // Return relative path
        }
    }
}

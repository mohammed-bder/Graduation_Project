using Graduation_Project.Api.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.Shared
{

    public class UploadController : BaseApiController
    {


        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost("image")]
        public async Task<ActionResult> UploadImage(IFormFile  file , string folder = "general" )
        {

            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponse(404, "File is required."));

            // validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if(!allowedExtensions.Contains(extension))
                return BadRequest(new ApiResponse(404, "Invalid file format. Allowed formats: .jpg, .jpeg, .png, .gif, .bmp, .webp"));


            // Validate File Size (Max 5MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxFileSize)
                return BadRequest(new ApiResponse(404, "File size must be less than 5MB."));


            // Validate Folder Name
            if (string.IsNullOrWhiteSpace(folder))
                return BadRequest(new ApiResponse(404, "Folder name is required."));


            string uniqueFileName = $"{Guid.NewGuid()}{extension}";

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folder);


            // ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string filPath = Path.Combine(uploadsFolder, uniqueFileName);

            using(var fileStream = new FileStream(filPath , FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string relativePath = $"/uploads/{folder}/{uniqueFileName}";

            return Ok(new { filePath = relativePath });

        }

    }
}

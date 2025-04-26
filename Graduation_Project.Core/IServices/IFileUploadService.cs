using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IFileUploadService
    {
        Task<(bool Success, string Message, string? FilePath)> UploadFileAsync(IFormFile file, string folderName ,ClaimsPrincipal? user , string? customFileName = null);

        Task<(bool Success, string Message)> DeleteFile(string relativePath);

        string CombinePath(string relativePath);

        string getRelativePath(string absolutePath);
    }
}

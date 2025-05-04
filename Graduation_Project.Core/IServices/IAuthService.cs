using Graduation_Project.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
        Task<UserDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}

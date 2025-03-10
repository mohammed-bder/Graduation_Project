using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IUserService
    {
        Task<AppUser> GetCurrentUserAsync();
        Task<object> GetCurrentBusinessUserAsync(string userId,UserRoleType userRole);
        public Task<string> GetUserRoleAsync(AppUser appUser);
    }
}

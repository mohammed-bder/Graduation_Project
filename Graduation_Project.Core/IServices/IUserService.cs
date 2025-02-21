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
        public Task<string> GetUserRoleAsync(AppUser appUser);
    }
}

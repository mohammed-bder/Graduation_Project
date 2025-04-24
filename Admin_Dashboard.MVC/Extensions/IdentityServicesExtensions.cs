
using Graduation_Project.Core.IServices;
using Graduation_Project.Repository.Identity;
using System.Text;
using Graduation_Project.Service;
using Graduation_Project.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Admin_Dashboard.MVC.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthService), typeof(AuthServices));
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IUserService), typeof(UserService));
            
            services.AddScoped(typeof(IPatientService), typeof(PatientServcie));

            // add Identity Services configuration (UserManager , SigninManager , RoleManager)
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;

            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            return services;
        }
    }
}

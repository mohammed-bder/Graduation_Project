using Graduation_Project.Core;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.IServices;
using Graduation_Project.Repository;
using Graduation_Project.Service;
using Microsoft.AspNetCore.Mvc;

namespace Secretary_Dashboard.MVC.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddLogging(config =>
            {
                config.AddConsole(); // Enables console logging
                config.AddDebug();   // Enables debug output
            });

            /****************************** Generic Respository Register ********************************/
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //unitOfWork replaces GenericRepository
            //this line is equivalent to the following line

            //services.AddScoped(typeof(GenericNoBaseEntityRepository<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}

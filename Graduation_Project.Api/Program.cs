using Graduation_Project.Api.Extensions;
using Graduation_Project.Api.Middlewares;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace Graduation_Project.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Service
            // Add services to the container.

            builder.Services.AddControllers();

            /****************************** Add Swagger Services********************************/
            builder.Services.AddSwaggerServices();

            /****************************** Connection String ********************************/
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            /****************************** Add Application Services ********************************/
            builder.Services.AddApplicationServices();

            #endregion

            var app = builder.Build();

            #region Update-Database auto 
            var scope = app.Services.CreateScope();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //Create Object from ApplicationDbContext using CLR Exiplicitly

            var factoryLogger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            //Create Object from ILoggerFactory using CLR Exiplicitly

            try
            {
                await applicationDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = factoryLogger.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occured While update Database..");
            }
            #endregion

            #region Configure kestrel Middlewares

            //Middlewares of Exception Handling 
            app.UseMiddleware<ExceptionMiddleware>();

            // Not found Endpoint
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}

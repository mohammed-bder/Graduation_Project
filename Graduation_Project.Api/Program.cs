using Graduation_Project.Api.Controllers.Account;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /****************************** Generic Respository Register ********************************/
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            /****************************** Connection String ********************************/
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            /****************************** Identityuser and IdentityRole Services ********************************/
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });
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
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}

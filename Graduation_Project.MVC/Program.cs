//using Graduation_Project.Api.Extensions;
//using Graduation_Project.Api.Filters;
//using Graduation_Project.Api.Middlewares;
//using Graduation_Project.Core.Models.Identity;
//using Graduation_Project.Repository.Data;
//using Graduation_Project.Repository.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Graduation_Project.MVC
//{
//    public class Program
//    {
//        public async   void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddControllersWithViews();




//            /****************************** Connection String ********************************/
//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//            {
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//            });

//            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
//            {
//                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
//            });


//            ///****************************** Global Connection String ********************************/
//            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
//            //{
//            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DeploymentDbGlobal"));
//            //});

//            //builder.Services.AddDbContext<AppIdentityDbContext>(options =>
//            //{
//            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DeploymentIdentityDbGlobal"));
//            //});


//            builder.Services.AddScoped(typeof(ExistingIdFilter<>));
//            /****************************** Add Application Services ********************************/
//            builder.Services.AddApplicationServices();

//            builder.Services.AddIdentityServices(builder.Configuration);



//            var app = builder.Build();


//            #region Update-Database auto 
//            var scope = app.Services.CreateScope();

//            var services = scope.ServiceProvider;

//            var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
//            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();


//            //Create Object from ApplicationDbContext using CLR Exiplicitly

//            var factoryLogger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
//            //Create Object from ILoggerFactory using CLR Exiplicitly

//            try
//            {
//                await applicationDbContext.Database.MigrateAsync(); // for automatically update database
//                await ApplicationDbContextSeed.SeedAsync(applicationDbContext); // for seeding entered data

//                await _identityDbContext.Database.MigrateAsync(); // for automatically update database


//                var _userManager = services.GetRequiredService<UserManager<AppUser>>(); // Ask CLR to create object from UserManager Explicitly
//                await AppIdentityDbContextSeed.SeedUserAsync(_userManager); // for seeding entered data


//                var _roleDbContext = services.GetRequiredService<RoleManager<IdentityRole>>();
//                await RoleSeed.RoleSeedAsync(_roleDbContext); // for seeding entered role
//            }
//            catch (Exception ex)
//            {
//                var logger = factoryLogger.CreateLogger<Program>();
//                logger.LogError(ex, "An Error Occured While update Database..");
//            }
//            #endregion


//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }

//            //Middlewares of Exception Handling 
//            app.UseMiddleware<ExceptionMiddleware>();

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthentication();
//            app.UseAuthorization();



//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=Home}/{action=Index}/{id?}");

//            app.Run();
//        }
//    }
//}

using Graduation_Project.Api.Controllers.Account;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Repository;
using Graduation_Project.Repository.Repository.Interfaces.Clinics;
using Graduation_Project.Repository.Repository.Interfaces.Doctors;
using Graduation_Project.Repository.Repository.Interfaces.Patients;
using Graduation_Project.Repository.Repository.Interfaces.Pharmacies;
using Graduation_Project.Repository.Repository.Interfaces.Shared;
using Graduation_Project.Repository.Repository.Repos.Clinics;
using Graduation_Project.Repository.Repository.Repos.Doctors;
using Graduation_Project.Repository.Repository.Repos.Patients;
using Graduation_Project.Repository.Repository.Repos.Pharmacies;
using Graduation_Project.Repository.Repository.Repos.Shared;
using Microsoft.EntityFrameworkCore;
using System;

namespace Graduation_Project.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /****************************** Generic Refactored ********************************/
            //builder.Services.AddScoped<IGenericRepository<Secretary>, GenericRepository<Secretary>>();
            //builder.Services.AddScoped<IGenericRepository<Doctor>, GenericRepository<Doctor>>();
            //builder.Services.AddScoped<IGenericRepository<Patient>, GenericRepository<Patient>>();
            //builder.Services.AddScoped<IGenericRepository<Prescription>, GenericRepository<Prescription>>();


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            /****************************** Doctor Services ********************************/
            builder.Services.AddScoped<ISecretaryRepository, SecretaryRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPharmacistRepository, PharmacistRepository>();
            builder.Services.AddScoped<RegistrationService>();

            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IEducationRepository, EducationRepository>();
            builder.Services.AddScoped<IDoctorClinicRepository, DoctorClinicRepository>();
            builder.Services.AddScoped<IDoctorSubspecialityRepository, DoctorSubspecialityRepository>();
            builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
            builder.Services.AddScoped<ISubSpecialitiesRepository, SubSpecialitiesRepository>();

            /****************************** Shared Services ********************************/
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();

            /****************************** Patient ********************************/


            /****************************** Connection String ********************************/
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            /****************************** Identityuser and IdentityRole Services ********************************/
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });


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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

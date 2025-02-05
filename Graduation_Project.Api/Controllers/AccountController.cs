using Graduation_Project.Api.DTO.Account;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.API.Dtos.Account;

namespace Graduation_Project.Api.Controllers
{
  
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authServices;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authServices , 
            ApplicationDbContext applicationDbContext,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
           _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        // login End Point
        [HttpPost("login")] // POST: api/account/login
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            // 1. check if email is founded
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            var userDto = new UserDTO()
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
            };

            return Ok(userDto);
        }



        [HttpPost("DoctorRegister")] // post: api/account/DoctorRegister
        public async Task<ActionResult<UserDTO>> DoctorRegister(DoctorRegisterDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Split(" ").Length != 2 )
                return BadRequest(new ApiResponse(400, "Full Name must include first and last name."));

            var user = new AppUser()
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                UserType = UserType.Doctor,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to create user."));


            // Fetch registered user
            var registeredUser = await _userManager.FindByEmailAsync(model.Email);

            if (registeredUser == null)
                return BadRequest(new ApiResponse(400, "User registration failed."));


            
            var newDoctor = new Doctor()
            {
                FirstName = model.FullName.Split(" ")[0],
                LastName = model.FullName.Split(" ")[1],
                ApplicationUserId = registeredUser.Id,
                ConsultationFees = model.ConsultationFees,
                Gender = model.Gender
            };

            try
            {
                // Add Doctor to the application database
                await _applicationDbContext.Doctors.AddAsync(newDoctor);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while creating doctor for user: {Email}", model.Email);


                // Rollback: If doctor creation fails, delete the user from identity DB
                await _userManager.DeleteAsync(registeredUser);

                return BadRequest(new ApiResponse(500, "An unexpected error occurred."));

            }
            _logger.LogInformation("Doctor registered successfully: {Email}", model.Email);

            return Ok(new UserDTO()
            {
                FullName = user.UserName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
            });

        }


        [HttpPost("PatientRegister")] // post: api/account/PatientRegister
        public async Task<ActionResult<UserDTO>> PatientRegister(PatientRegisterDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Split(' ').Length != 2)
                return BadRequest(new ApiResponse(500, "you must enter first name and last name."));

            var user = new AppUser()
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                UserType = UserType.Patient,
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to create user."));


            // Fetch registered user
            var registeredPatient = await _userManager.FindByEmailAsync(model.Email);

            if (registeredPatient == null)
                return BadRequest(new ApiResponse(400, "User registration failed."));


            var newPatient = new Patient()
            {
                FirstName = model.FullName.Split(' ')[0],
                LastName = model.FullName.Split(' ')[1],
                Gender = model.Gender,
                ApplicationUserId = registeredPatient.Id
            };

            try
            {
                await _applicationDbContext.Patients.AddAsync(newPatient);
                await _applicationDbContext.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while creating patient for user: {Email}", model.Email);


                // Rollback: If doctor creation fails, delete the user from identity DB
                await _userManager.DeleteAsync(registeredPatient);

                return BadRequest(new ApiResponse(500, "An unexpected error occurred."));
            }

            _logger.LogInformation("Patient registered successfully: {Email}", model.Email);

            return Ok(new UserDTO
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Token = await _authServices.CreateTokenAsync(user, _userManager),
                });
        }



        [Authorize]
        [HttpGet] // GET: api/Account/GetCurrentUser
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
            });
        }
    }
}

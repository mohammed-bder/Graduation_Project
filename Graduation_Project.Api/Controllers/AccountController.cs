using Graduation_Project.Api.DTO.Account;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IRepositories;
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
        private readonly IGenericRepository<Doctor> _doctorRepo;
        private readonly IGenericRepository<Clinic> _clinicRepo;
        private readonly IGenericRepository<DoctorClinic> _doctorClinicRepo;
        private readonly IGenericRepository<Patient> _patientRepo;
        private readonly IGenericRepository<Specialty> _specialtyRepo;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authServices , 
            IGenericRepository<Doctor> doctorRepo,
            IGenericRepository<Clinic> clinicRepo,
            IGenericRepository<DoctorClinic> doctorClinicRepo,
            IGenericRepository<Patient> patientRepo,
            IGenericRepository<Specialty> specialtyRepo,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
            _logger = logger;
            _doctorRepo = doctorRepo;
            this._clinicRepo = clinicRepo;
            this._doctorClinicRepo = doctorClinicRepo;
            _patientRepo = patientRepo;
            _specialtyRepo = specialtyRepo;
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

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var userDto = new UserDTO()
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                Role = role
            };

            return Ok(userDto);
        }



        [HttpPost("DoctorRegister")] // post: api/account/DoctorRegister
        public async Task<ActionResult<UserDTO>> DoctorRegister(DoctorRegisterDTO model)
        {

            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() {Errors = new string[] { "This Email is Already Exist" } });

            //if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Split(" ").Length != 2 )
            //    return BadRequest(new ApiResponse(400, "Full Name must include first and last name."));

            var user = new AppUser()
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
            };

            var result = await _userManager.CreateAsync(user, model.Password);

           await _userManager.AddToRoleAsync(user, UserRoleType.Doctor.ToString());
            
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to create user."));

            // Fetch registered user
            var registeredUser = await _userManager.FindByEmailAsync(model.Email);

            if (registeredUser == null)
                return BadRequest(new ApiResponse(400, "User registration failed."));

           
            // split the full name into first and last name
            var nameParts = model.FullName?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new string[0];


           var specialty = await _specialtyRepo.GetAsync(model.SpecialtyId);
            
            if(specialty is  null)
            {
                // Rollback: If doctor creation fails, delete the user from identity DB
                await _userManager.DeleteAsync(registeredUser);
                return BadRequest(new ApiResponse(400, "please enter valid specialty"));
            }
            var newDoctor = new Doctor()
            {
                FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty,
                LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty,
                ApplicationUserId = registeredUser.Id,
                ConsultationFees = model.ConsultationFees,
                Gender = model.Gender,
                SpecialtyId = model.SpecialtyId,
                Specialty = await _specialtyRepo.GetAsync(model.SpecialtyId),
            };

            var newClinic = new Clinic()
                {
                Name = model.ClinicName,
                RegionId = model.RegionId,
            };



            try
            {
                // Add Doctor to the application database
                var createdDoctor =   await _doctorRepo.AddWithSaveAsync(newDoctor);
                //await _doctorRepo.SaveAsync();

                var createdClinic = await _clinicRepo.AddWithSaveAsync(newClinic);
                //await _clinicRepo.SaveAsync();

                // Use the IDs directly after saving
                var newDoctorClinic = new DoctorClinic
                {
                    DoctorId = createdDoctor.Id,
                    ClinicId = createdClinic.Id
                };

                await _doctorClinicRepo.AddAsync(newDoctorClinic);
                await _doctorClinicRepo.SaveAsync();


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
                FullName = user.FullName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                Role = UserRoleType.Doctor.ToString()
            });

        }


        [HttpPost("PatientRegister")] // post: api/account/PatientRegister
        public async Task<ActionResult<UserDTO>> PatientRegister(PatientRegisterDTO model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is Already Exist" } });

            //if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Split(' ').Length != 2)
            //    return BadRequest(new ApiResponse(500, "you must enter first name and last name."));

            var user = new AppUser()
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to create user."));


            // Fetch registered user
            var registeredPatient = await _userManager.FindByEmailAsync(model.Email);

            await _userManager.AddToRoleAsync(user, UserRoleType.Patient.ToString());

            if (registeredPatient == null)
                return BadRequest(new ApiResponse(400, "User registration failed."));

            // split the full name into first and last name
            var nameParts = model.FullName?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            var newPatient = new Patient()
            {
                FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty,
                LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty,
                Gender = model.Gender,
                ApplicationUserId = registeredPatient.Id
            };

            try
            {
                await _patientRepo.AddAsync(newPatient);
                await _patientRepo.SaveAsync();

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
                    Role = UserRoleType.Patient.ToString()
                });
        }



        [Authorize]
        [HttpGet] // GET: api/Account/GetCurrentUser
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return Ok(new UserDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                Role = role
            });
        }

        [HttpGet("EmailExists")] // GET: api/Account/EmailExists
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        [HttpPost("logout")] // POST: api/Account/logout
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ApiResponse(200, "Logged out successfully."));
        }
    }
}

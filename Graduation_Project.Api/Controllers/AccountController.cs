using Graduation_Project.Api.DTO.Account;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using System.Security.Cryptography;
using Talabat.API.Dtos.Account;
using System.Security.Cryptography;
using Graduation_Project.Core.DTOs;
using Graduation_Project.Service;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.RateLimiting;

namespace Graduation_Project.Api.Controllers
{
  
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authServices;
        private readonly IGenericRepository<Doctor> _doctorRepo;
        private readonly IGenericRepository<Clinic> _clinicRepo;
      
        private readonly IGenericRepository<Patient> _patientRepo;
        private readonly IGenericRepository<Specialty> _specialtyRepo;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authServices , 
            IGenericRepository<Doctor> doctorRepo,
            IGenericRepository<Clinic> clinicRepo,
           
            IGenericRepository<Patient> patientRepo,
            IGenericRepository<Specialty> specialtyRepo,
            ILogger<AccountController> logger,
            IUserService userService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
            _doctorRepo = doctorRepo;
            this._clinicRepo = clinicRepo;
        
            _patientRepo = patientRepo;
            _specialtyRepo = specialtyRepo;
        }

        [EnableRateLimiting("LoginRateLimit")]
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginDTO model)
        {
            // 1. Check if email exists
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Invalid Email"));


            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            // Get the current business user (Doctor or Patient)
            var BusinessUser = await _userService.GetCurrentBusinessUserAsync(user.Id,
                (UserRoleType)Enum.Parse(typeof(UserRoleType), role));

            // Generate JWT Token
            var token = await _authServices.CreateTokenAsync(user, _userManager);

            // Generate or Retrieve Active Refresh Token
            var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if (refreshToken == null)
            {
                refreshToken = TokenHelper.GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            // Set refresh token in cookie
            SetRefreshTokenInCookie(refreshToken.Token, refreshToken.ExpiresOn);

            // Handle Doctor
            if (BusinessUser is Doctor doctor)
            {
                var doctorDto = new DoctorDTO()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Token = token,
                    Role = role,
                    Speciality = doctor.Specialty.Name_ar,
                    Description = doctor.Description,
                    PictureUrl = doctor.PictureUrl,
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.ExpiresOn,
                    IsAuthenticated = true
                };
                return Ok(doctorDto);
            }
            // Handle Patient
            else if (BusinessUser is Patient patient)
            {
                var patientDto = new PatientDTO()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Token = token,
                    Role = role,
                    PictureUrl = patient.PictureUrl,
                    BloodType = patient.BloodType,
                    Points = patient.Points,
                    Age = patient.DateOfBirth != null ? DateTime.Now.Year - patient.DateOfBirth.Value.Year : null,
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.ExpiresOn,
                    IsAuthenticated = true
                };
                return Ok(patientDto);
            }

            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
        }

        [HttpPost("DoctorRegister")] // post: api/account/DoctorRegister
        public async Task<ActionResult<UserDTO>> DoctorRegister(DoctorRegisterDTO model)
        {

            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() {Errors = new string[] { "This Email is Already Exist" } });

            if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Split(" ").Length != 2)
                return BadRequest(new ApiResponse(400, "Full Name must include first and last name."));

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

            var OTP = GenerateOtp(registeredUser); // private method to generate OTP Random Code
            await _userManager.UpdateAsync(registeredUser);

            var emailsent = await _emailService.SendEmailAsync(model.Email, "Your OTP Code", $"Your OTP code is {OTP}");
            if(!emailsent)
                return StatusCode(500,new ApiResponse(500, "Failed to send OTP code to your email."));
           
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
                SlotDurationMinutes = 20
            };

            try
            {
                // Add Doctor to the application database
              var regDoctor =  await _doctorRepo.AddWithSaveAsync(newDoctor);

                var newClinic = new Clinic()
                {
                    Name = model.ClinicName,
                    RegionId = model.RegionId,
                    DoctorId = regDoctor.Id
                };

                await _clinicRepo.AddWithSaveAsync(newClinic);
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
                //Token = await _authServices.CreateTokenAsync(user, _userManager),
                Role = UserRoleType.Doctor.ToString()
            });
        }

        [HttpPost("PatientRegister")] // post: api/account/PatientRegister
        public async Task<ActionResult<UserDTO>> PatientRegister(PatientRegisterDTO model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is Already Exist" } });

            if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Split(' ').Length != 2)
                return BadRequest(new ApiResponse(500, "you must enter first name and last name."));

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

            var OTP = GenerateOtp(registeredPatient); // private method to generate OTP Random Code
            await _userManager.UpdateAsync(registeredPatient);

            var emailsent = await _emailService.SendEmailAsync(model.Email, "Your OTP Code", $"Your OTP code is {OTP}");
            if (!emailsent)
                return StatusCode(500, new ApiResponse(500, "Failed to send OTP code to your email."));

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
                Points = 0,
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
                    //Token = await _authServices.CreateTokenAsync(user, _userManager),
                    Role = UserRoleType.Patient.ToString()
                });
        }


        /******************************** Get Current User ********************************/
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

        /******************************** Logout ********************************/
        [HttpPost("logout")] // POST: api/Account/logout
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { message = "No refresh token found" });

            var result = await _authServices.RevokeTokenAsync(refreshToken);
            if (!result)
                return BadRequest(new { message = "Failed to revoke token" });

            Response.Cookies.Delete("refreshToken");
            await _signInManager.SignOutAsync();

            return Ok(new { message = "Logged out successfully" });
        }

        /******************************** Verify OTP ********************************/
        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Invalid Email"));

            if (user.OtpCode != model.OtpCode || user.OtpExpiry < DateTime.Now)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Invalid OTP"));

            user.EmailConfirmed = true;
            user.OtpCode = null;
            user.OtpExpiry = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to verify OTP"));

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Email verified successfully"));
        }

        /******************************** Resend OTP ********************************/
        [EnableRateLimiting("OtpRateLimit")]
        [HttpPost("ResendOTP")]
        public async Task<IActionResult> ResendOTP([FromBody] VerifyOtpRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Invalid Email"));

            if (user.EmailConfirmed)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Email is already verified."));

            if (user.OtpCode is not null && user.OtpExpiry > DateTime.Now.AddMinutes(-2))
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "You can request a new OTP after 2 minutes."));

            var OTP = GenerateOtp(user); // private method to generate OTP Random Code

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to resend OTP"));

            var emailSent = await _emailService.SendEmailAsync(model.Email, "Your New OTP Code", $"Your new OTP code is {OTP}");
            if (!emailSent)
                return StatusCode(500, new ApiResponse(500, "Failed to send new OTP code"));

            return Ok(new ApiResponse(StatusCodes.Status200OK, "New OTP sent successfully"));
        }



        /******************************** Refresh Token ********************************/
        [HttpGet("RefreshToken")] // GET: api/Account/refreshToken
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Refresh token is missing"));

            var result = await _authServices.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, result.Message));

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        /******************************** Revoke Token ********************************/
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Token is required"));

            var result = await _authServices.RevokeTokenAsync(token);

            if (!result)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Token is invalid"));

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Token revoked"));
        }

        /******************************** Forgot Password ********************************/
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Email not found"));

            var OTP = GenerateOtp(user);
            await _userManager.UpdateAsync(user);

            var emailSent = await _emailService.SendEmailAsync(request.Email, "Reset Password OTP", $"Your OTP code is {OTP}");
            if (!emailSent)
                return StatusCode(500, new ApiResponse(500, "Failed to send OTP code to your email."));

            return Ok(new ApiResponse(StatusCodes.Status200OK, "OTP sent successfully"));
        }
        /******************************** Reset Password ********************************/
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Email not found"));

            if(user.OtpCode != request.OTP || user.OtpExpiry < DateTime.Now)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid OTP"));

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
            user.OtpCode = null;
            user.OtpExpiry = null;

            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Password reset successfully" });
        }
        /******************************** Change Password ********************************/
        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword()
        //{

        //}
        /******************************** Private Method ********************************/
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookiOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookiOption);
        }

        private string GenerateOtp(AppUser user)
        {
            var OTP = new Random().Next(100000, 999999).ToString();
            user.OtpCode = OTP;
            user.OtpExpiry = DateTime.Now.AddMinutes(5);

            return OTP;
        }
    }
}

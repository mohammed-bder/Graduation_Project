using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IServices;
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

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authServices , ApplicationDbContext applicationDbContext)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
           _applicationDbContext = applicationDbContext;
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
            var user = new AppUser()
            {
                FullName = model.FullName,
                //PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                UserType = UserType.Doctor,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            var registeredUser = await _userManager.FindByEmailAsync(model.Email);
            //var registeredUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newDoctor = new Doctor()
            {
                FirstName = model.FullName.Split(" ")[0],
                LastName = model.FullName.Split(" ")[1],
                ApplicationUserId = registeredUser.Id,
                ConsultationFees = model.ConsultationFees,
                //NationalID = model.NationalID,
                Gender = model.Gender
            };

           await _applicationDbContext.Doctors.AddAsync(newDoctor);
            try
            {
                
            await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log exception
                Console.WriteLine(ex.Message);
                // Optionally, rethrow or handle accordingly
            }

            return Ok(new UserDTO()
            {
                FullName = user.UserName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
            });

        }
    }
}

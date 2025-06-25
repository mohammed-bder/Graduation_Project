using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.DTOs;
using Graduation_Project.Core.Enums;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Patients;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Graduation_Project.Core.Specifications.PharmacySpecifications;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Service
{
    public class AuthServices : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public AuthServices(IConfiguration configuration,
            IUnitOfWork unitOfWork ,
            UserManager<AppUser> userManager,
            IUserService userService
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            // 1. Private Clam (User-Defined)
            var authClams = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName , user.UserName),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.NameIdentifier , user.Id)
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClams.Add(new Claim(ClaimTypes.Role, role));
            }

            switch (userRoles.FirstOrDefault())
            {
                case nameof(UserRoleType.Doctor):
                    //Get Doctor From Doctor Table in business DB
                    var doctorSpecification = new DoctorByAppUserIdSpecs(user.Id);
                    var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorSpecification);
                    authClams.Add(new Claim(Identifiers.DoctorId, doctor.Id.ToString()));
                    break;

                case nameof(UserRoleType.Patient):
                    //Get Patient From Patient Table in business DB
                    var patientSpecs = new PatientByAppUserIdSpecs(user.Id);
                    var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpecs);
                    authClams.Add(new Claim(Identifiers.PatientId, patient.Id.ToString()));
                    break;


                case nameof(UserRoleType.Pharmacist):
                    //Get Pharmacist From Pharmacist Table in business DB
                    var pharmacistSpecs = new PharmacistByAppUserIdSpecs(user.Id);
                    var pharmacist = await _unitOfWork.Repository<Pharmacy>().GetWithSpecsAsync(pharmacistSpecs);
                    authClams.Add(new Claim(Identifiers.PharmacyId, pharmacist.Id.ToString()));
                    break;


                default:
                    break;
            }

            // generate secret key
            //Encoding.UTF8.GetBytes => we use this to transform from string to array of byte 
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));


            var token = new JwtSecurityToken(
                // Payload
                // 1. Registered claims
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),

                // 2. Private claims
                claims: authClams,

                // 3. secret key
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /******************************************* Refresh Token Async *********************************************************/
        public async Task<UserDto> RefreshTokenAsync(string token)
        {

            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token)); // Get user by refresh token (Any ==> it will return true if the user has any refresh token that matches the token passed in)

            if (user == null)
                return new UserDto { IsAuthenticated = false, Message = "Invalid refresh token." };

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token); // Get the refresh token from the user

            if (!refreshToken.IsActive) // Check if the refresh token is active
                return new UserDto { IsAuthenticated = false , Message = "Inactive refresh token." };

            //refreshToken.RevokedOn = DateTime.UtcNow;

            //var newRefreshToken = TokenHelper.GenerateRefreshToken();
            //user.RefreshTokens.Add(newRefreshToken);
            //await _userManager.UpdateAsync(user);

            var newRefreshToken = refreshToken;
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);


            var jwtToken = await CreateTokenAsync(user, _userManager);

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var businessUser = await _userService.GetCurrentBusinessUserAsync(user.Id, (UserRoleType)Enum.Parse(typeof(UserRoleType), role));

            if (businessUser is Doctor doctor)
            {
                var doctorDto =  new DoctorDTO
                {
                    IsAuthenticated = true,
                    Token = jwtToken,
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpiration = newRefreshToken.ExpiresOn,
                    Role = role,
                    FullName = user.FullName,
                    Email = user.Email,
                    Speciality = doctor.Specialty.Name_ar,
                    Description = doctor.Description,
                    PictureUrl = doctor.PictureUrl
                };
                return doctorDto;
            }
            else if (businessUser is Patient patient)
            {
                var pateintDto =  new PatientDTO
                {
                    IsAuthenticated = true,
                    Token = jwtToken,
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpiration = newRefreshToken.ExpiresOn,
                    Role = role,
                    FullName = user.FullName,
                    Email = user.Email,
                    PictureUrl = patient.PictureUrl,
                    BloodType = patient.BloodType,
                    Age = patient.DateOfBirth != null ? DateTime.Now.Year - patient.DateOfBirth.Value.Year : null,
                    Points = patient.Points
                };
                return pateintDto;
            }

            return new UserDto { IsAuthenticated = false };
        }

        /******************************************* Revoke Token Async *********************************************************/
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            // Revoke the old refresh token and generate a new one
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;
        }
    }
}

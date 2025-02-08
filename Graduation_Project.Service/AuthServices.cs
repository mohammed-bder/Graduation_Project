using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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

        public AuthServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            //throw new NotImplementedException();

            // 1. Private Clam (User-Defined)
            var authClams = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName , user.UserName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClams.Add(new Claim(ClaimTypes.Role, role));
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
    
    }
}

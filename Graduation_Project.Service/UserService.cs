using Graduation_Project.Core;
using Graduation_Project.Core.Enums;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Patients;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Graduation_Project.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<AppUser> userManager , IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<object> GetCurrentBusinessUserAsync(string userId, UserRoleType userRole)
        {
            // switch on the role 
            switch (userRole) 
            {
                // get the doctor or patient from business DB
                case UserRoleType.Doctor:
                    var doctorSpec = new DoctorWithSpecialitySpecs(userId);
                    var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorSpec);
                    return doctor;

                case UserRoleType.Patient:
                    var patientSpec = new PatientForProfileSpecs(userId);
                    var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpec);
                    return patient;

                default:
                    break;
            }

            return false;
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return null; // No authenticated user found
            }

            var currentUser = await _userManager.FindByEmailAsync(email);
            if (currentUser == null)
            {
                return null; // User not found
            }

            return currentUser;
        }

        public async Task<string> GetUserRoleAsync(AppUser appUser)
        {
            var roles = await _userManager.GetRolesAsync(appUser);

            return roles.FirstOrDefault(); // Assuming a user has only one role
        }

    }
}
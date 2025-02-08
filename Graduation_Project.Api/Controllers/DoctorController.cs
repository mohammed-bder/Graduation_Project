using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO.Doctor;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.APIs.Helpers;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{
    public class DoctorController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Doctor> _genericRepository;
        private readonly IMapper _mapper;

        public DoctorController(UserManager<AppUser> userManager,IGenericRepository<Doctor> genericRepository
                                , IMapper mapper)
        {
            _userManager = userManager;
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("GetProfile")]
        public async Task<ActionResult<DoctorForProfileDto>> GetDoctorProfile()
        {
            // Get Current User
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            //Get Doctor From Doctor Table in business DB
            DoctorSpecification doctorSpecification = new DoctorSpecification(user.Id);

            var doctor = await _genericRepository.GetWithSpecsAsync(doctorSpecification);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // Map to DoctorForProfileDto
            var doctorForProfileDto = new DoctorForProfileDto()
            {
                Email = email,
                FullName = doctor.FirstName + " " + doctor.LastName,
                PhoneNumber = doctor.PhoneNumber,
                Gender = doctor.Gender.ToString(),
                ConsultationFees = doctor.ConsultationFees,
                DateOfBirth = doctor.DateOfBirth,
                PictureUrl = doctor.PictureUrl,
                Description = doctor.Description
            };

            return Ok(doctorForProfileDto);
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPost("SaveProfile")]
        public async Task<ActionResult<DoctorForProfileDto>> EditDoctorProfile(DoctorForProfileDto doctorDtoFromRequest)
        {
            // Get Current User 
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            //Get Doctor From Doctor Table in business DB
            DoctorSpecification doctorSpecification = new DoctorSpecification(user.Id);
            var doctor = await _genericRepository.GetWithSpecsAsync(doctorSpecification);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            //split fullName Ex: mohamed hazem kamal --> ["mohamed","hazem","kamal"]
            var nameParts = doctorDtoFromRequest.FullName?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            // Edit                                         
            doctor.FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            doctor.LastName = nameParts.Length > 1 ? string.Join(" ",nameParts.Skip(1)) : string.Empty;
            doctor.PhoneNumber = doctorDtoFromRequest.PhoneNumber;
            doctor.DateOfBirth = doctorDtoFromRequest.DateOfBirth;
            doctor.Description = doctorDtoFromRequest.Description;
            doctor.ConsultationFees = doctorDtoFromRequest.ConsultationFees;
            doctor.PictureUrl = doctorDtoFromRequest.PictureUrl;

            // Update Business DB
            _genericRepository.Update(doctor);
            await _genericRepository.SaveAsync();

            return Ok("Created Sucssefully");

        }
    }
}



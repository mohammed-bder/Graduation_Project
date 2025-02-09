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

namespace Graduation_Project.Api.Controllers.DoctorControllers
{
    public class DoctorController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Doctor> _genericRepository;
        private readonly IMapper _mapper;

        public DoctorController(UserManager<AppUser> userManager, IGenericRepository<Doctor> genericRepository
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
            DoctorForProfileSpecs doctorSpecification = new DoctorForProfileSpecs(user.Id);

            var doctor = await _genericRepository.GetWithSpecsAsync(doctorSpecification);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // Map to DoctorForProfileDto
            var doctorForProfileDto = new DoctorForProfileDto()
            {
                Email = email,
            };

            doctorForProfileDto = _mapper.Map(doctor, doctorForProfileDto);

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
            DoctorForProfileSpecs doctorSpecification = new DoctorForProfileSpecs(user.Id);
            var doctor = await _genericRepository.GetWithSpecsAsync(doctorSpecification);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // mapping 
            doctor = _mapper.Map(doctorDtoFromRequest, doctor);

            // Update Business DB
            _genericRepository.Update(doctor);
            await _genericRepository.SaveAsync();

            return Ok("Created Sucssefully");

        }
    }
}



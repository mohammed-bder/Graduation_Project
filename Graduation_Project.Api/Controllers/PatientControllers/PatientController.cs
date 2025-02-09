using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO.Patient;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.PatientControllers
{
    public class PatientController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Patient> _genericRepository;
        private readonly IMapper _mapper;

        public PatientController(UserManager<AppUser> userManager, IGenericRepository<Patient> genericRepository, IMapper mapper)
        {
            _userManager = userManager;
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetProfile")]
        public async Task<ActionResult<PatientForProfileDto>> GetProfile()
        {
            // Get current user 
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            // Get current patient from business DB
            var patientSpecs = new PatientForProfileSpecs(user.Id);
            var patient = await _genericRepository.GetWithSpecsAsync(patientSpecs);
            if (patient == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // mapping for Dto
            var patientForProfileDto = new PatientForProfileDto
            {
                Email = email
            };

            patientForProfileDto = _mapper.Map(patient, patientForProfileDto);

            return Ok(patientForProfileDto);
        }

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPost("EditProfile")]
        public async Task<ActionResult<PatientForProfileDto>> EditProfile(PatientForProfileDto patientProfileFromRequest)
        {
            // Get current user 
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            // Get current patient from business DB
            var patientSpecs = new PatientForProfileSpecs(user.Id);
            var patient = await _genericRepository.GetWithSpecsAsync(patientSpecs);
            if (patient == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // mapping for Patient 
            patient = _mapper.Map(patientProfileFromRequest, patient);

            // Update Patient 
            _genericRepository.Update(patient);
            await _genericRepository.SaveAsync();

            return Ok("Updated Successfully");
        }
    }
}

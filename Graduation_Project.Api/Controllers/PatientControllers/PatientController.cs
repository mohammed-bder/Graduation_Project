using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PatientController(UserManager<AppUser> userManager,
                                IMapper mapper,
                                IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpecs);
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
        [HttpPut("EditProfile")]
        public async Task<ActionResult<PatientForProfileDto>> EditProfile(PatientForProfileDto patientProfileFromRequest)
        {
            // Get current user 
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            // Get current patient from business DB
            var patientSpecs = new PatientForProfileSpecs(user.Id);
            var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpecs);
            if (patient == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // mapping for Patient 
            patient = _mapper.Map(patientProfileFromRequest, patient);

            // Update Patient 
            _unitOfWork.Repository<Patient>().Update(patient);
            await _unitOfWork.Repository<Patient>().SaveAsync();

            return Ok(patientProfileFromRequest);
        }

        /****************************************** Medicl Category ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetMedicalCategory")]
        public async Task<ActionResult<IReadOnlyList<MedicalCategoryDto>>> GetAllMediclCategory()
        {
            var MediclaCategories = await _unitOfWork.Repository<MedicalCategory>().GetAllAsync();
            if (MediclaCategories is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Medical Categories Not Found"));
            }

            return Ok(_mapper.Map<IReadOnlyList<MedicalCategory>, IReadOnlyList<MedicalCategoryDto>>(MediclaCategories));
        }

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<MedicalCategoryDto>> GetMediclCategory(int Id)
        {
            var MedicalCategory = await _unitOfWork.Repository<MedicalCategory>().GetAsync(Id);
            if(MedicalCategory is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Medical Category Not Found"));
            }
            return Ok(_mapper.Map<MedicalCategory, MedicalCategoryDto>(MedicalCategory));
        }
    }
}

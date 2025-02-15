using AutoMapper;
using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace Graduation_Project.Api.Controllers.PatientControllers
{
    public class MedicalHistoryController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public MedicalHistoryController(IUnitOfWork unitOfWork , IMapper mapper , UserManager<AppUser> userManager , IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }

        /****************************************** Get All Medicl History ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetMedicalHistory")]
        public async Task<ActionResult<IReadOnlyList<MedicalHistoryDto>>> GetAllMediclHistory()
        {
            var spec = new MedicalHistoryWithPatientAndCategory();
            var MedicalHistories = await _unitOfWork.Repository<MedicalHistory>().GetAllWithSpecAsync(spec);
            if (MedicalHistories is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Medical Histories Not Found"));
            }
            return Ok(_mapper.Map<IReadOnlyList<MedicalHistory>, IReadOnlyList<MedicalHistoryDto>>(MedicalHistories));
        }

        /****************************************** Get Medicl History By Id ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<MedicalHistoryInfoDto>> GetMedicalHistory(int Id)
        {
            var spec = new MedicalHistoryWithPatientAndCategory(Id);
            var medicalHistory = await _unitOfWork.Repository<MedicalHistory>().GetWithSpecsAsync(spec);
            if (medicalHistory is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Medical History Not Found"));
            }
            return Ok(_mapper.Map<MedicalHistory, MedicalHistoryInfoDto>(medicalHistory));
        }

        /****************************************** Get All Medical History and Categories for Current User ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetAllHistoryAndCategories")]
        public async Task<ActionResult<IReadOnlyList<MedicalHistoryDto>>> GetAllHistoryAndCategories()
        {
            var currentUser = await _userService.GetCurrentUserAsync();

            var spec = new PatientForProfileSpecs(currentUser.Id);
            var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(spec);

            if (patient is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "This Patient Not Found"));

            var medicalHistoriesSpec = new MedicalHistoryWithCategorySpec(patient.Id);
            var medicalHistories = await _unitOfWork.Repository<MedicalHistory>().GetAllWithSpecAsync(medicalHistoriesSpec);

            var medicalHistoriesDto = _mapper.Map<IReadOnlyList<MedicalHistory>, IReadOnlyList<MedicalHistoryDto>>(medicalHistories);

            return Ok(medicalHistoriesDto);
        }

        /****************************************** Add Medicl History ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPost("MedicalHistory")]
        public async Task<ActionResult<MedicalHistoryFormDto>> AddMedicalHistory(MedicalHistoryFormDto model)
        {
            var currentUser = await _userService.GetCurrentUserAsync();

            var spec = new PatientForProfileSpecs(currentUser.Id);
            var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(spec);

            if (patient is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "This Patient Not Found"));

            var medicalHistory = _mapper.Map<MedicalHistoryFormDto, MedicalHistory>(model);
            medicalHistory.PatientId = patient.Id;

            try
            {
                await _unitOfWork.Repository<MedicalHistory>().AddAsync(medicalHistory);
                var result = await _unitOfWork.CompleteAsync();
                return Ok(_mapper.Map<MedicalHistory, MedicalHistoryFormDto>(medicalHistory));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }

        /****************************************** Edit Medicl History ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPut("MedicalHistory/{Id:int}")]
        public async Task<ActionResult<MedicalHistoryInfoDto>> EditMedicalHistory(MedicalHistoryFormDto model,int Id)
        {
            var medicalHistory = await _unitOfWork.Repository<MedicalHistory>().GetAsync(Id);
            if (medicalHistory == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Medical History Not Found"));

            var medicalHistoryMapped = _mapper.Map(model, medicalHistory);
            try
            {
                _unitOfWork.Repository<MedicalHistory>().Update(medicalHistoryMapped);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to update Medical History"));
                }
                return Ok(_mapper.Map<MedicalHistory, MedicalHistoryFormDto>(medicalHistory));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }


        /****************************************** Delete Medical History  ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpDelete("MedicalHistory/{Id:int}")]
        public async Task<ActionResult> DeleteMedicalHistory(int Id)
        {
            var medicalHistory = await _unitOfWork.Repository<MedicalHistory>().GetAsync(Id);
            if (medicalHistory is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Medical History Not Found"));

            try
            {
                _unitOfWork.Repository<MedicalHistory>().Delete(medicalHistory);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to delete Medical History"));
                }
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Medical History Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }
    }
}

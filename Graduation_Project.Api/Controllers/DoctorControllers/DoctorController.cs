using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Doctor;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Helpers;
using Graduation_Project.APIs.Helpers;
using Graduation_Project.Core;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.FavouriteSpecifications;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Graduation_Project.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos.Account;

namespace Graduation_Project.Api.Controllers.DoctorControllers
{
    public class DoctorController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorController(UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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

            var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorSpecification);
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
        [HttpPost("EditProfile")]
        public async Task<ActionResult<DoctorForProfileDto>> EditDoctorProfile(DoctorForProfileDto doctorDtoFromRequest)
        {
            // Get Current User 
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            //Get Doctor From Doctor Table in business DB
            DoctorForProfileSpecs doctorSpecification = new DoctorForProfileSpecs(user.Id);
            var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorSpecification);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // mapping 
            doctor = _mapper.Map(doctorDtoFromRequest, doctor);

            // Update Business DB
            _unitOfWork.Repository<Doctor>().Update(doctor);
            await _unitOfWork.Repository<Doctor>().SaveAsync();

            return Ok(doctorDtoFromRequest);

        }

        //[Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("DoctorWithFilter")]
        public async Task<ActionResult<Pagination<Doctor>>> GetDoctorsAsync([FromQuery] DoctorSpecParams specParams)
        {
            //var doctorSpecification = new SortingDoctorWithSpecificaiton(sort);
            var doctorSpecification = new SortingDoctorWithSpecificaiton(specParams);
            var doctors = await _unitOfWork.Repository<Doctor>().GetAllWithSpecAsync(doctorSpecification);
            var data = _mapper.Map<IReadOnlyList<Doctor>, IReadOnlyList<SortingDoctorDto>>(doctors);
            var countSpec = new DoctorWithFilterCountSpecification(specParams);
            var count = await _unitOfWork.Repository<Doctor>().GetCountAsync(countSpec);

            return Ok(new Pagination<SortingDoctorDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }


        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetDetailsDuringAppointment/{id:int}")]
        public async Task<ActionResult<DoctorDetailsDto>> GetDoctorDetailsDuringAppointment(int id)
        {
            //Get Doctor From Doctor Table in business DB
            DoctorDetailsSpecs doctorSpecification = new DoctorDetailsSpecs(id);
            var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorSpecification);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // Get Current Patient To Know the doctor is Favourite or not
            var email = User.FindFirstValue(ClaimTypes.Email);

            // Map to doctorDetailsDto
            var doctorDetailsDto = new DoctorDetailsDto()
            {
                NumberOfPatients = doctor.Appointments.Count(),
                IsFavourite = await CheckFavouriteDoctor(email, doctor.Id)  // check in favourite table 
            };

            doctorDetailsDto = _mapper.Map(doctor, doctorDetailsDto);

            return Ok(doctorDetailsDto);
        }


        // check in favourite table 
        private async Task<bool> CheckFavouriteDoctor(string patientEmail , int docId)
        {
            var user = await _userManager.FindByEmailAsync(patientEmail);
            var patientSpecs = new PatientForProfileSpecs(user.Id);
            //return Ok(new Pagination<SortingDoctorDto>());

            var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpecs);
            // Fav Specs
            var favouriteSpecs = new FavouriteSpecs(docId, patient.Id);
            var favouriteDoctor = await _unitOfWork.Repository<Favorite>().GetWithSpecsAsync(favouriteSpecs);
            if (favouriteDoctor == null)
                return false;

            return true;
        }

        
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetAbout/{id:int}")]
        public async Task<ActionResult<DoctorAboutDto>> GetDoctorAbout(int id)
        {
            // Get Doctor From Db include  education , clinics
            var specs = new DoctorWithEducationAndClinicsSpecs(id);
            var doctorFromDb = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(specs);

            // Check if The doctor Is Exist ??
            if (doctorFromDb == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // Map To DoctorAboutDto
            var doctorAboutDto = new DoctorAboutDto();

            // Assign Doctor Info (Description & Subspeciality)
            doctorAboutDto.Description = doctorFromDb.Description;
            doctorAboutDto.DoctorSubspeciality = doctorFromDb.DoctorSubspeciality.Select(d => d.SubSpecialities.Name).ToList();

            // Assign Doctor Education
            doctorAboutDto = _mapper.Map(doctorFromDb.Education, doctorAboutDto);

            // Assign Doctor Clinics List
            doctorAboutDto.DoctorClinics = _mapper.Map(doctorFromDb.DoctorClincs.Select(c => c.Clinic), doctorAboutDto.DoctorClinics);

            return Ok(doctorAboutDto);
        }
    }

}



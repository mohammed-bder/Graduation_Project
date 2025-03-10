using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Api.DTO.FeedBacks;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Api.Helpers;
using Graduation_Project.APIs.Helpers;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.FavouriteSpecifications;
using Graduation_Project.Core.Specifications.FeedBackSpecifications;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorController(UserManager<AppUser> userManager
                                , IUnitOfWork unitOfWork
                                , IUserService userService
                                , IMapper mapper)
        {
            _userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("GetProfile")]
        public async Task<ActionResult<DoctorForProfileToReturnDto>> GetDoctorProfile()
        {
            // Get Current Doctor Id
            var DoctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            //Get Doctor From Doctor Table in business DB
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(DoctorId);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            var email = User.FindFirstValue(ClaimTypes.Email);
            // Map to DoctorForProfileDto
            var doctorForProfileToReturnDto = new DoctorForProfileToReturnDto()
            {
                Email = email,
            };

            doctorForProfileToReturnDto = _mapper.Map(doctor, doctorForProfileToReturnDto);

            return Ok(doctorForProfileToReturnDto);
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPut("EditProfile")]
        public async Task<ActionResult<DoctorForProfileDto>> EditDoctorProfile(DoctorForProfileDto doctorDtoFromRequest)
        {
            // Get Current Doctor Id
            var DoctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));

            //Get Doctor From Doctor Table in business DB
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(DoctorId);
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

        //**************************************************** Doctor From Patient ****************************************************//

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetDetailsDuringAppointment/{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<ActionResult<DoctorDetailsDto>> GetDoctorDetailsDuringAppointment(int id)
        {
            //Get Doctor From Doctor Table in business DB
            DoctorDetailsSpecs doctorSpecification = new DoctorDetailsSpecs(id);
            var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorSpecification);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // Get Current Patient To Know the doctor is Favourite or not
            var PatientId = int.Parse(User.FindFirstValue(Identifiers.PatientId));

            // Map to doctorDetailsDto
            var doctorDetailsDto = new DoctorDetailsDto()
            {
                NumberOfPatients = doctor.Appointments.Count(),
                IsFavourite = await CheckFavouriteDoctor(PatientId, doctor.Id)  // check in favourite table 
            };

            doctorDetailsDto = _mapper.Map(doctor, doctorDetailsDto);

            return Ok(doctorDetailsDto);
        }


        // check in favourite table 
        private async Task<bool> CheckFavouriteDoctor(int patientid , int docId)
        {
            var favouriteSpecs = new FavouriteSpecs(docId, patientid);
            var favouriteDoctor = await _unitOfWork.Repository<Favorite>().GetWithSpecsAsync(favouriteSpecs);
            if (favouriteDoctor == null)
                return false;

            return true;
        }

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetAbout/{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<ActionResult<DoctorAboutDto>> GetDoctorAbout(int id)
        {
            // Get Doctor From Db include  education , clinics
            var specs = new DoctorWithEducationAndClinicsSpecs(id);
            var doctorFromDb = await _unitOfWork.Repository<Doctor>().GetWithSpecsAsync(specs);

            // Check if The doctor Is Exist ??
            if (doctorFromDb == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            if (doctorFromDb.Clinic == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound,"Doctor clinic not found"));
            // Map To DoctorAboutDto
            var doctorAboutDto = new DoctorAboutDto();

            // Assign Doctor Info (Description & Subspeciality)
            doctorAboutDto.Description = doctorFromDb.Description;
            doctorAboutDto.DoctorSubspeciality = doctorFromDb.DoctorSubspeciality.Select(d => d.SubSpecialities.Name_en).ToList();

            // Assign Doctor Education
            doctorAboutDto = _mapper.Map(doctorFromDb.Education, doctorAboutDto);

            // Assign Doctor Clinics List
            //doctorAboutDto = _mapper.Map(doctorFromDb.Clinic, doctorAboutDto);
            //doctorAboutDto = _mapper.Map(doctorFromDb.Clinic, doctorAboutDto.Clinic);
            doctorAboutDto.Name = doctorFromDb.Clinic.Name;
            doctorAboutDto.Location = doctorFromDb.Clinic.Address;
            doctorAboutDto.LocationLink = doctorFromDb.Clinic.LocationLink;
            doctorAboutDto.PictureUrl = doctorFromDb.Clinic.PictureUrl;

            return Ok(doctorAboutDto);
        }


        
    }

}



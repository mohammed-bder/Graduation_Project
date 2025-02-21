using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.DoctorSubSpecialitySpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Graduation_Project.Api.Controllers.DoctorControllers
{
    public class EducationController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EducationController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet]
        public async Task<ActionResult<EducationDto>> GetEducation()
        {
            // get cuurent user
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));

            // get Current Doctor from buisness DB with include education
            DoctorWithEducationSpecs doctorForProfileSpecs = new DoctorWithEducationSpecs(doctorId);
            var doctor = await unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorForProfileSpecs);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // map doctor ( speciality , subspeciality) manually
            EducationDto educationDto = new EducationDto()
            {
                SpecializationId = doctor.SpecialtyId,
                Sub_Specilaities_IDs = doctor.DoctorSubspeciality != null ? doctor.DoctorSubspeciality.Select(d => d.SubSpecialitiesId).ToArray() : null ,
                ExperianceYears = doctor.ExperianceYears
            };
            // map doctor ( Education ) automapper
            educationDto = mapper.Map(doctor.Education, educationDto);

            return Ok(educationDto);
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPut]
        public async Task<ActionResult<EducationDto>> EditEducation(EducationDto educationDtoFromRequest)
        {
            // Get current doctorId
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));

            // Get current doctor from business DB with included education
            DoctorWithEducationSpecs doctorForProfileSpecs = new DoctorWithEducationSpecs(doctorId);
            var doctor = await unitOfWork.Repository<Doctor>().GetWithSpecsAsync(doctorForProfileSpecs);
            if (doctor == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // Assign doctor (Experience - Specialty)
            doctor.ExperianceYears = educationDtoFromRequest.ExperianceYears;
            doctor.SpecialtyId = educationDtoFromRequest.SpecializationId;

            // Education mapping
            var education = new Education();
            education = mapper.Map(educationDtoFromRequest, education);
            education.DoctorId = doctor.Id;

            doctor.Education = education;

            // Update Doctor (Education - Experience - Specialty)
            unitOfWork.Repository<Doctor>().Update(doctor);

            // Assign doctor and subspecialties in M-M table if they exist
            if (educationDtoFromRequest.Sub_Specilaities_IDs != null)
            {
                // Get existing subspecialties in DB
                var doctorSubSpecialitySpecs = new DoctorSubSpecialitySpecs(doctor.Id);
                var existingSubSpecialities = await unitOfWork.Repository<DoctorSubspeciality>().GetAllWithSpecAsync(doctorSubSpecialitySpecs);

                if (existingSubSpecialities != null) // Past subspecialties will be updated
                {
                    // Select existing IDs only
                    var existingSubSpecialitiesIds = existingSubSpecialities.Select(e => e.SubSpecialitiesId).ToHashSet();

                    // Determine items to be removed
                    var listToBeRemoved = existingSubSpecialities.Where(e => !educationDtoFromRequest.Sub_Specilaities_IDs.Contains(e.SubSpecialitiesId));

                    // Delete removed items
                    unitOfWork.Repository<DoctorSubspeciality>().DeleteRange(listToBeRemoved);
                    await unitOfWork.Repository<DoctorSubspeciality>().SaveAsync();

                    // Add new items that were not previously existing
                    var listToBeAdded = educationDtoFromRequest.Sub_Specilaities_IDs
                        .Where(id => !existingSubSpecialitiesIds.Contains(id))
                        .Select(id => new DoctorSubspeciality
                        {
                            SubSpecialitiesId = id,
                            DoctorId = doctor.Id
                        }).ToList();

                    if (listToBeAdded.Count > 0)
                        await unitOfWork.Repository<DoctorSubspeciality>().AddRangeAsync(listToBeAdded);
                }
                else // Subspecialties will be created
                {
                    var listToBeAdded = educationDtoFromRequest.Sub_Specilaities_IDs
                        .Select(id => new DoctorSubspeciality
                        {
                            SubSpecialitiesId = id,
                            DoctorId = doctor.Id
                        }).ToList();

                    if (listToBeAdded.Count > 0)
                        await unitOfWork.Repository<DoctorSubspeciality>().AddRangeAsync(listToBeAdded);
                }
            }

            await unitOfWork.Repository<Doctor>().SaveAsync();
            await unitOfWork.Repository<DoctorSubspeciality>().SaveAsync();

            return Ok(educationDtoFromRequest);
        }
    }
}

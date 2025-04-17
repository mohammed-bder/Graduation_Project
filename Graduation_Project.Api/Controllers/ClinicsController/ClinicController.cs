using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Clinics;
using Graduation_Project.Core.Specifications.ClinicsSpecifications;
using Graduation_Project.Repository;
using Graduation_Project.Repository.Data.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.Api.Controllers.ClinicsController
{

    public class ClinicController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploadService _fileUploadService;

        public ClinicController( IMapper mapper ,
            UserManager<AppUser> userManager ,
            IUnitOfWork unitOfWork ,
            IFileUploadService fileUploadService
            )
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._unitOfWork = unitOfWork;
            this._fileUploadService = fileUploadService;
        }



        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("ClinicInfo")]
        public async Task<ActionResult<ClinicInfoToReturnDTO>> GetClinicForCurrentDoctor()
        {
            var doctorId =  int.Parse(User.FindFirstValue(Identifiers.DoctorId)!);

     

            var clinic = await GetClinicForDoctor(doctorId);

            if (clinic is null)
                return BadRequest(new ApiResponse(404 ));

            return Ok(_mapper.Map<Clinic, ClinicInfoToReturnDTO>(clinic));
        }


        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPut("Edit")]
        public async Task<ActionResult<ClinicInfoToReturnDTO>> Edit( ClinicEditDTO model)
            {
            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId)!);


            var clinicFromDB = await GetClinicForDoctor(doctorId);

            if (clinicFromDB is null)
                return NotFound(new ApiResponse(404, "Clinic not found"));


            var regSpec = new RegionWithGovDataSpecification(model.RegionId);
            var reg = await _unitOfWork.Repository<Region>().GetWithSpecsAsync(regSpec);
            model.GovernorateId = reg.governorateId;
                

            _mapper.Map(model , clinicFromDB);


            var newClinicPicture = new ClinicPictures();

            var clinicPicCount = new ClinicPictureCountSpecification();
            var numberOfClinicPicture =  await _unitOfWork.Repository<ClinicPictures>().GetCountAsync(clinicPicCount);

            if(numberOfClinicPicture == 3 && model.ImageFile is not null)
            {
                return BadRequest(new ApiResponse(400, "you reach max number of images {3}"));
            }

            if (model.ImageFile is not null)
            {

                (bool Success, string Message, string? FilePath) =  await _fileUploadService.UploadFileAsync(model.ImageFile, "Doctor/ClinicPicture", User);
               
                if(!Success)
                {
                    return BadRequest(new ApiResponse(400, Message));
                }


                newClinicPicture.ImageUrl = FilePath;
                newClinicPicture.ClinicId = clinicFromDB.Id;
              

                if (clinicFromDB.ClinicPictures == null)
                    clinicFromDB.ClinicPictures = new List<ClinicPictures>();

                clinicFromDB.ClinicPictures.Add(newClinicPicture);

                await _unitOfWork.Repository<ClinicPictures>().AddAsync(newClinicPicture);
            }
            // Generate Google Maps link
            //if (model.Latitude != 0 && model.Longitude != 0)
            //{
            //    string mapsLink = $"https://www.google.com/maps?q={model.Latitude},{model.Longitude}";
            //    clinicFromDB.LocationLink = mapsLink;
            //}

            // Update LocationLink if provided
            if (!string.IsNullOrWhiteSpace(model.LocationLink))
            {
                clinicFromDB.LocationLink = model.LocationLink;
            }
         

            // Validate ClinicType
            if (model.Type is not null && !Enum.IsDefined(typeof(ClinicType), model.Type.Value))
            {
                return BadRequest(new ApiResponse(404, "Clinic Type Not Found"));
            }


            try
            {

                
                _unitOfWork.Repository<Clinic>().Update(clinicFromDB);
              
               var result =  await _unitOfWork.CompleteAsync();

                var returnDTO = _mapper.Map<ClinicInfoToReturnDTO>(clinicFromDB);
                returnDTO.GovernorateId = reg.governorateId;
                returnDTO.GovernorateName = reg.governorate.Name_en;

                return Ok(new
                {
                    message = "Clinic updated successfully",
                    Data = returnDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }


        }


        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPost("addContactInfo")]
        public async Task<ActionResult> AddContactInfo(string phoneNumber)
        {

            try
            {
                var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId)!);


                var clinicRepository = _unitOfWork.Repository<Clinic>();
                var contactNumberRepository = _unitOfWork.Repository<ContactNumber>();

                // Get clinic associated with doctor
                var spec = new ClinicByDoctorIdSpecification(doctorId);
                var clinic = await clinicRepository.GetWithSpecsAsync(spec);

                if (clinic is null)
                    return NotFound(new ApiResponse(404, "Clinic not found for this doctor."));

                // Check if phone number already exists
                var phoneSpec = new ClinicContactByNumber(phoneNumber);
                var existingPhone = await contactNumberRepository.GetWithSpecsAsync(phoneSpec);

                if (existingPhone is not null)
                    return Conflict(new ApiResponse(409, "Phone number is already in use."));


                var newContactInfo = new ContactNumber()
                {
                    PhoneNumber = phoneNumber,
                    ClinicId = clinic.Id
                };

                await contactNumberRepository.AddAsync(newContactInfo);
                var result = await _unitOfWork.CompleteAsync();

                if (result == 0)
                    return BadRequest(new ApiResponse(400, "Failed to add phone number."));


                return Ok(new { message = $"Phone number {phoneNumber} added successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
          

        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpDelete("ContactInfo")]
        public async Task<ActionResult> DeleteContactInfo(string number)
        {
            var contactInfoRepository = _unitOfWork.Repository<ContactNumber>();

            var spec = new ClinicContactByNumber(number);
            var contactNumber =  await contactInfoRepository.GetWithSpecsAsync(spec);
            
            //var contactNumber =  await contactInfoRepository.GetAsync(id);

            if (contactNumber is null)
                return BadRequest(new ApiResponse(400, "this number does't exist"));


            #region get clinic related to this phone number

            var doctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId)!);


            var clinic = await GetClinicForDoctor(doctorId);

            if (clinic is null) 
                return NotFound(new ApiResponse(404, "Clinic not found"));

            if (clinic.Id != contactNumber.ClinicId)
                return BadRequest(new ApiResponse(400, "you are not auth"));

            #endregion


            contactInfoRepository.Delete(contactNumber);

            var result = await _unitOfWork.CompleteAsync();

            if (result == 0)
                return BadRequest(new ApiResponse(400, "can't delete phone number"));

            return Ok(new ApiResponse(200 , "phone number deleted Successfully"));
            
        }



        private async Task<Clinic?> GetClinicForDoctor(int doctorId)
        {
            var spec = new ClinicByDocIdWithAllDataSpecification(doctorId);
            return await _unitOfWork.Repository<Clinic>().GetWithSpecsAsync(spec);
        }

    }
}

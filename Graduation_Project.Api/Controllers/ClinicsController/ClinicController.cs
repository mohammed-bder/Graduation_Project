using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Specifications.ClinicsSpecifications;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.ClinicsController
{

    public class ClinicController : BaseApiController
    {
        private readonly IGenericRepository<Clinic> _clinicRepo;
        private readonly IMapper _mapper;

        public ClinicController(IGenericRepository<Clinic> clinicRepo , IMapper mapper)
        {
            this._clinicRepo = clinicRepo;
            this._mapper = mapper;
        }


        [HttpGet] 
        public async Task<ActionResult< IReadOnlyList<ClinicInfoDTO>>> GetAll()
        {

            var spec = new ClinicWithAllDataSpecification();
            var clinics = await _clinicRepo.GetAllWithSpecAsync(spec);


            return Ok(_mapper.Map<IReadOnlyList<Clinic>, IReadOnlyList<ClinicInfoDTO>>(clinics));
        }

        [HttpGet("Id/{id}")]
        public async Task<ActionResult<ClinicInfoDTO>> GetById(int id)
        {
            var spec = new ClinicWithAllDataSpecification(id);
            var clinic = await _clinicRepo.GetWithSpecsAsync(spec);

            if (clinic is null)
                return BadRequest(new ApiResponse(404, $"clinic with id = {id} not found"));

            return Ok(_mapper.Map<Clinic, ClinicInfoDTO>(clinic));
        }


        [HttpPost("Edit/{id}")]
        public async Task<ActionResult<ClinicInfoDTO>> Edit(int id, ClinicEditDTO model)
        {
            var spec = new ClinicWithAllDataSpecification(id);
            var clinicFromDB = await _clinicRepo.GetWithSpecsAsync(spec);


            if (model.Longitude != clinicFromDB.Longitude || model.Latitude != clinicFromDB.Latitude)
            {
                clinicFromDB.Longitude = model.Longitude;
                clinicFromDB.Latitude = model.Latitude;
                
                // Generate Google Maps link
                string mapsLink = $"https://www.google.com/maps?q={model.Latitude},{model.Longitude}";
                clinicFromDB.LocationLink = mapsLink;
            }



            if (clinicFromDB is null)
                return BadRequest(new ApiResponse(404, $"clinic with id = {id} not found"));

            if (model.Id == 0 || model.Id != clinicFromDB.Id)
                return BadRequest(new ApiResponse(404, $"you can't change clinic Id"));

            if (model.Name == null || model.Name != clinicFromDB.Name)
                clinicFromDB.Name = model.Name!;

            if (model.PictureUrl == null || model.PictureUrl != clinicFromDB.PictureUrl)
                clinicFromDB.PictureUrl = model.PictureUrl!;

            if (model.Address == null || model.Address != clinicFromDB.Address)
                clinicFromDB.Address = model.Address!;

            if (model.LocationLink == null || model.LocationLink != clinicFromDB.LocationLink)
                clinicFromDB.LocationLink = model.LocationLink!;


            
            var isTypeExist =   Enum.IsDefined<ClinicType>(model.Type!.Value);

            if (!isTypeExist)
                return BadRequest(new ApiResponse(404, "Clinic Type Not Found"));

            if (model.Type == null || model.Type != clinicFromDB.Type)
                clinicFromDB.Type = model.Type!;

            if (model.RegionId != clinicFromDB.RegionId)
                clinicFromDB.RegionId = model.RegionId;



            try
            {
                _clinicRepo.Update(clinicFromDB);
                await _clinicRepo.SaveAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "can't change clinic data"));
            }

            return Ok(new
            {
                message = $"clinic with id = {id} updated Successfully",
                Data = model
            });


        }
    }
}

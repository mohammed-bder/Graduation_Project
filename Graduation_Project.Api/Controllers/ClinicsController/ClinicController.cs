using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
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
        public async Task<ActionResult< IReadOnlyList<ClinicDTO>>> GetAll()
        {

            var spec = new ClinicWithAllDataSpecification();
            var clinics = await _clinicRepo.GetAllWithSpecAsync(spec);

            return Ok(_mapper.Map<IEnumerable<Clinic>, IEnumerable<ClinicDTO>>(clinics));
        }
    }
}

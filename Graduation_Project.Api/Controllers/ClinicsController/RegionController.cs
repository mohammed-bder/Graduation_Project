using AutoMapper;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Specifications.ClinicsSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.ClinicsController
{

    public class RegionController : BaseApiController
    {
        private readonly IGenericRepository<Region> _regionRepo;
        private readonly IGenericRepository<Governorate> _governorateRepo;
        private readonly IMapper _mapper;

        public RegionController(
            IGenericRepository<Region> regionRepo,
            IGenericRepository<Governorate> governorateRepo,
            IMapper mapper
            )
        {
            _regionRepo = regionRepo;
            _governorateRepo = governorateRepo;
            this._mapper = mapper;
        }

        // get all Region
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RegionDTO>>> GetAll()
        {
            var regions = await _regionRepo.GetAllAsync();

            if (regions is null)
                return BadRequest(new ApiResponse(404, "there is no Region"));

            var regionDTOs = new List<RegionDTO>();

            foreach (var item in regions)
                regionDTOs.Add(_mapper.Map<Region , RegionDTO>(item));

            return Ok(regionDTOs);
        }


        // get  Region by id
        [HttpGet("{id}")]
        public async Task<ActionResult<RegionDTO>> GetById(int id)
        {
            var region = await _regionRepo.GetAsync(id);
            if (region is null)
                return BadRequest(new ApiResponse(404, $"there is no region with id={id}"));


            return Ok(_mapper.Map<Region, RegionDTO>(region));
        }

        // get region by Government id
        [HttpGet("govId/{id}")]
        public async Task<ActionResult<List<RegionDTO>>> GetByGovId(int id)
        {

            var spec = new RegionWithGovDataSpecification(id);
            var regions = await _regionRepo.GetAllWithSpecAsync(spec);

            if(regions.IsNullOrEmpty())
            {
                return BadRequest(new ApiResponse(404, $"there is no region with this Governorate id= {id}"));
            }

            var regionsDTO = new List<RegionDTO>();
            foreach (var item in regions)
                regionsDTO.Add(_mapper.Map<Region, RegionDTO>(item));


            return Ok(regionsDTO);
        }

    }
}

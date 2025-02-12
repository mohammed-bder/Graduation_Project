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

        public RegionController(
            IGenericRepository<Region> regionRepo,
            IGenericRepository<Governorate> governorateRepo
            )
        {
            _regionRepo = regionRepo;
            _governorateRepo = governorateRepo;
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
            {
                var regionDTO = new RegionDTO()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                regionDTOs.Add(regionDTO);
            }
            return Ok(regionDTOs);
        }


        // get  Region by id
        [HttpGet("{id}")]
        public async Task<ActionResult<RegionDTO>> GetById(int id)
        {
            var region = await _regionRepo.GetAsync(id);
            if (region is null)
                return BadRequest(new ApiResponse(404, $"there is no region with id={id}"));

            var regionDTO = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name
            };
            return Ok(regionDTO);
        }

        // get region by Government id
        [HttpGet("govId/{id}")]
        public async Task<ActionResult<List<RegionDTO>>> GetByGovId(int id)
        {

            var spec = new RegionWithGovDataSpecification(id);
            var regions = await _regionRepo.GetAllWithSpecAsync(spec);

            var regionsDTO = new List<RegionDTO>();
            foreach (var item in regions)
            {
                var regionDTO = new RegionDTO()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                regionsDTO.Add(regionDTO);
            }

            return Ok(regionsDTO);
        }

    }
}

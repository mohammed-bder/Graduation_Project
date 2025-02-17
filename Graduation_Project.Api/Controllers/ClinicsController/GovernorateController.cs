using AutoMapper;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models.Clinics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.ClinicsController
{

    public class GovernorateController : BaseApiController
    {
        private readonly IGenericRepository<Governorate> _governorateRepo;
        private readonly IMapper _mapper;

        public GovernorateController(
            IGenericRepository<Governorate> governorateRepo,
            IMapper mapper
            )
        {
            _governorateRepo = governorateRepo;
            this._mapper = mapper;
        }


        // get all Governorate
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GovernorateDTO>>> GetAll()
        {
            var governorates = await _governorateRepo.GetAllAsync();

            if (governorates is null)
                return BadRequest( new ApiResponse(404, "there is no Governorate"));
                    
            var governoratesDTO = new List< GovernorateDTO>();
            foreach (var item in governorates)
            {
                governoratesDTO.Add(_mapper.Map<Governorate , GovernorateDTO>(item));
            }
            return Ok(governoratesDTO);
        }

        // get  Governorate by id
        [HttpGet("{id}")]
        public async Task<ActionResult<GovernorateDTO>> GetById(int id)
        {
            var governorate = await _governorateRepo.GetAsync(id);
            if (governorate is null)
                return BadRequest(new ApiResponse(404, $"there is no governorate with id={id}"));

            return Ok(_mapper.Map<Governorate, GovernorateDTO>(governorate));
        }
    }
}

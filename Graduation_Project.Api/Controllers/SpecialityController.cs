using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{
    public class SpecialityController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SpecialityController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<SpecialityDTO>> GetAllSpecialities()
        {
            //there is no specification for the speciality
            var specialities = await unitOfWork.Repository<Specialty>().GetAllAsync();
            return Ok(mapper.Map<IEnumerable<Specialty>, IEnumerable<SpecialityDTO>>(specialities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialityDTO>> GetSpecialityById(int id)
        {
            //there is no specification for the speciality
            var speciality = await unitOfWork.Repository<Specialty>().GetAsync(id);
            if (speciality == null)
            {
                return NotFound(new ApiResponse(404));   //404
            }
            return Ok(mapper.Map<Specialty, SpecialityDTO>(speciality));
        }

    }
}

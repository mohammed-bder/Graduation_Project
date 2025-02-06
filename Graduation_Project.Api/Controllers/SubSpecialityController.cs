using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{
    public class SubSpecialityController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SubSpecialityController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<SubSpecialityDTO>> GetAllSubSpeciality()
        {
            var spec = new SubSpecialityWithSpecialtySpecification();
            var SubSpeciality = await unitOfWork.Repository<SubSpecialities>().GetAllWithSpecAsync(spec);
            return Ok(mapper.Map<IEnumerable<SubSpecialities>, IEnumerable<SubSpecialityDTO>>(SubSpeciality));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubSpecialityDTO>> GetSubSpecialityById(int id)
        {
            var spec = new SubSpecialityWithSpecialtySpecification(id);
            var SubSpeciality = await unitOfWork.Repository<SubSpecialities>().GetWithSpecsAsync(spec);
            if (SubSpeciality == null)
            {
                return NotFound(new ApiResponse(404));   //404
            }
            return Ok(mapper.Map<SubSpecialities, SubSpecialityDTO>(SubSpeciality));
        }
    }
}

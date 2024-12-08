using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Repository.Repository.Interfaces.Doctors;

namespace Graduation_Project.Api.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubSpecialityController : ControllerBase
    {
        private readonly ISubSpecialitiesRepository subSpecialitiesRepository;

        public SubSpecialityController(ISubSpecialitiesRepository subSpecialitiesRepository)
        {
            this.subSpecialitiesRepository = subSpecialitiesRepository;
        }

        /******************** Get All Sub Specialities ********************/
        [HttpGet]
        public IActionResult GetAll()
        {
            var subSpecialities = subSpecialitiesRepository.GetSubSpecialtyID_Name();
            if (subSpecialities == null || !subSpecialities.Any())
            {
                return NotFound("No Sub Specialities found.");
            }
            return Ok(subSpecialities);
        }

        /******************** Get Sub Speciality by id ********************/
        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var ExistsubSpeciality = subSpecialitiesRepository.GetSubSpecialtyDTO_Id(Id);
            if (ExistsubSpeciality == null)
            {
                return NotFound($"Sub Specialty with ID {Id} not found.");
            }
            return Ok(ExistsubSpeciality);
        }

        /******************** Add Sub Speciality  ********************/
        [HttpPost]
        public IActionResult Add([FromBody] SubSpecialityDTO subSpecialityFromRequest)
        {
            if (ModelState.IsValid)
            {
                var NewSubSpeciality = new SubSpecialities
                {
                    Name = subSpecialityFromRequest.Name
                };
                subSpecialitiesRepository.Add(NewSubSpeciality);
                subSpecialitiesRepository.Save();

                return Ok($"Sub Specialty '{NewSubSpeciality.Name}' Created Successfully.");
            }

            return BadRequest("Invalid data.Sub Specialty name is required.");
        }

        /******************** Edit Sub Speciality Information ********************/
        [HttpPut]
        public IActionResult Edit(int Id, [FromBody] SubSpecialityDTO SubSpecialityFromRequest)
        {
            if (ModelState.IsValid)
            {
                var ExistsubSpeciality = subSpecialitiesRepository.GetById(Id);
                if (ExistsubSpeciality == null)
                {
                    return NotFound($"Sub Specialty with ID {Id} not found.");
                }

                ExistsubSpeciality.Name = SubSpecialityFromRequest.Name;

                subSpecialitiesRepository.Update(ExistsubSpeciality);
                subSpecialitiesRepository.Save();

                return Ok(new
                {
                    Message = $"Specialty '{ExistsubSpeciality.Name}' Edited Successfully.",
                    Specialty = new
                    {
                        ExistsubSpeciality.Id,
                        SubSpecialityFromRequest.Name
                    }
                });
            }
            return BadRequest(ModelState);
        }

        /******************** Delete Sub Speciality ********************/
        [HttpDelete("{Id:int}")]
        public IActionResult Delete(int Id)
        {
            var ExistsubSpeciality = subSpecialitiesRepository.GetById(Id);
            if (ExistsubSpeciality == null)
            {
                return NotFound($"Sub Specialty with ID {Id} not found.");
            }

            subSpecialitiesRepository.Delete(ExistsubSpeciality);
            subSpecialitiesRepository.Save();

            return Ok($"Sub Specialty '{ExistsubSpeciality.Name}' Deleted Successfully.");
        }
    }
}

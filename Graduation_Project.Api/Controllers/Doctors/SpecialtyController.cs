using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Repository.Repository.Interfaces.Doctors;

namespace Graduation_Project.Api.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyRepository specialtyRepository;

        public SpecialtyController(ISpecialtyRepository specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }

        /******************** Get All specialties ********************/
        [HttpGet]
        public IActionResult GetAll()
        {
            var specialties = specialtyRepository.GetSpecialtyID_Name();

            if (specialties == null || !specialties.Any())
            {
                return NotFound("No specialties found.");
            }
            return Ok(specialties);
        }

        /******************** Get specialty by id ********************/
        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var specialty = specialtyRepository.GetSpecialtyDTO_Id(Id);
            if (specialty == null)
            {
                return NotFound($"Specialty with ID {Id} not found.");
            }
            return Ok(specialty);
        }


        /******************** Add specialty ********************/
        [HttpPost]
        public IActionResult Add([FromBody] SpecialtyDTO SpecialtyFromRequest)
        {
            if (ModelState.IsValid)
            {
                var Newspeciality = new Specialty
                {
                    Name = SpecialtyFromRequest.Name
                };

                specialtyRepository.Add(Newspeciality);
                specialtyRepository.Save();
                return Ok($"Specialty '{Newspeciality.Name}' Created Successfully.");
            }

            return BadRequest("Invalid data.Specialty name is required.");
        }

        /******************** Edit Specialty Information ********************/
        [HttpPut]
        public IActionResult Edit(int Id, [FromBody] SpecialtyDTO SpecialtyFromRequest)
        {
            if (ModelState.IsValid)
            {
                var Existspecialty = specialtyRepository.GetById(Id);

                if (Existspecialty == null)
                {
                    return NotFound($"Specialty with ID {Id} not found.");
                }

                Existspecialty.Name = SpecialtyFromRequest.Name;

                specialtyRepository.Update(Existspecialty);
                specialtyRepository.Save();
                return Ok(new
                {
                    Message = $"Specialty '{Existspecialty.Name}' Edited Successfully.",
                    Specialty = new
                    {
                        Existspecialty.Id,
                        Existspecialty.Name
                    }
                });
            }

            return BadRequest(ModelState);
        }

        /******************** Delete Specialty ********************/
        [HttpDelete("{Id:int}")]
        public IActionResult Delete(int Id)
        {
            var specialty = specialtyRepository.GetById(Id);
            if (specialty == null)
            {
                return NotFound($"Specialty with ID {Id} not found.");
            }

            specialtyRepository.Delete(specialty);
            specialtyRepository.Save();
            return Ok($"Specialty '{specialty.Name}' Deleted Successfully.");
        }
    }
}

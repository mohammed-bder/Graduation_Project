using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Core.IRepositories;

namespace Graduation_Project.Api.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IGenericRepository<Education> educationRepository;

        public EducationController(IGenericRepository<Education> educationRepository)
        {
            this.educationRepository = educationRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<EducationDto>? educations = educationRepository.GetAllWithout_Nav_Prop();
            if (educations == null)
            {
                return BadRequest();
            }

            return Ok(educations);
        }

        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            Education? education = educationRepository.GetById(Id);
            if (education == null)
            {
                return BadRequest("Invalid Id ");
            }

            return Ok(education);
        }

        [HttpPost]
        public IActionResult Add(EducationDto educationDto)
        {
            if (ModelState.IsValid)
            {
                if (Enum.IsDefined(typeof(DoctorDegree), educationDto.Degree))
                {
                    DoctorDegree degree = Enum.Parse<DoctorDegree>(educationDto.Degree, true);//map string ===> enum

                    Education education = new Education
                    {
                        Degree = degree,
                        Institution = educationDto.Institution,
                        GraduationDate = educationDto.GraduationDate,
                        Specialty = educationDto.Specialty,
                        DoctorId = educationDto.DoctorId,
                    };
                    educationRepository.Add(education);
                    educationRepository.Save();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid DoctorDegree");
                }
            }
            else
            {
                return BadRequest("Invalid Attributes");
            }
        }

        [HttpPut]
        public IActionResult Edit(EducationDto educationDto)
        {
            if (educationDto.Id == null)
            {
                return BadRequest("Invalid Id");
            }
            Education? educationFromDb = educationRepository.GetById((int)educationDto.Id);
            if (educationFromDb != null)
            {
                if (ModelState.IsValid)
                {
                    if (Enum.IsDefined(typeof(DoctorDegree), educationDto.Degree))
                    {
                        DoctorDegree degree = Enum.Parse<DoctorDegree>(educationDto.Degree, true);

                        educationFromDb.Degree = degree;
                        educationFromDb.Institution = educationDto.Institution;
                        educationFromDb.GraduationDate = educationDto.GraduationDate;
                        educationFromDb.Specialty = educationDto.Specialty;
                        educationFromDb.DoctorId = educationDto.DoctorId;

                        educationRepository.Update(educationFromDb);
                        educationRepository.Save();

                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Invalid DoctorDegree");
                    }
                }
                else
                {
                    return BadRequest("Invalid Attributes");
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Education? educationFromDb = educationRepository.GetById(id);
            if (educationFromDb != null)
            {
                educationRepository.Delete(educationFromDb);
                educationRepository.Save();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

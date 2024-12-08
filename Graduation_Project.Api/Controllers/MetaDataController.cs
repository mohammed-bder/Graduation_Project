using Graduation_Project.Repository.Repository.Interfaces.Doctors;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaDataController : ControllerBase
    {
        private readonly ISpecialtyRepository specialtyRepository;

        public MetaDataController(ISpecialtyRepository specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }

        /********************** Get All Doctor Degrees **********************/
        [HttpGet("DoctorDegrees")]
        public IActionResult GetDoctorDegrees()
        {
            string[] degrees = Enum.GetNames(typeof(DoctorDegree)); // ["Bachelor","Master","Doctorate","Other"]

            var response = new
            {
                MetaData = new
                {
                    TotalCount = degrees.Length,
                    Description = "List of doctor degrees."
                },
                Data = degrees
            };

            return Ok(response);
        }

        /********************** Get All genders **********************/
        [HttpGet("Gender")]
        public IActionResult Gender()
        {
            string[] degrees = Enum.GetNames(typeof(Gender));

            var response = new
            {
                MetaData = new
                {
                    TotalCount = degrees.Length,
                    Description = "List of genders."
                },
                Data = degrees
            };

            return Ok(response);
        }

        /********************** Get All Bold Type **********************/
        [HttpGet("BloodTypes")]
        public IActionResult BloodTypes()
        {
            string[] degrees = Enum.GetNames(typeof(BloodType));

            var response = new
            {
                MetaData = new
                {
                    TotalCount = degrees.Length,
                    Description = "List of blood types."
                },
                Data = degrees
            };

            return Ok(response);
        }

        /********************** Get All Clinic Type **********************/
        [HttpGet("ClinicTypes")]
        public IActionResult ClinicTypes()
        {
            string[] degrees = Enum.GetNames(typeof(ClinicType));

            var response = new
            {
                MetaData = new
                {
                    TotalCount = degrees.Length,
                    Description = "List of blood types."
                },
                Data = degrees
            };

            return Ok(response);
        }

        /********************** Get All Doctor Specialties **********************/
        [HttpGet("DoctorSpecialties")]
        public IActionResult GetDoctorSpecialties()
        {
            var Specialties = specialtyRepository.GetAllNames();

            if (Specialties == null || !Specialties.Any())
            {
                return NotFound("Doctor specialties not found.");
            }

            var response = new
            {
                MetaData = new
                {
                    TotalCount = Specialties.Count,
                    Description = "List of doctor specialties"
                },
                Data = Specialties
            };

            return Ok(response);
        }
    }
}

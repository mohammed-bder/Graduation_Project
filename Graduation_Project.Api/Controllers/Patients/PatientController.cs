using Graduation_Project.Repository.Repository.Interfaces.Patients;

namespace Graduation_Project.Api.Controllers.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        // Add a new patient
        [HttpPost]
        public IActionResult AddPatient([FromBody] Patient patient)
        {
            patientRepository.Add(patient);
            patientRepository.Save();
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
        }

        // Edit an existing patient
        [HttpPut("{id:int}")]
        public IActionResult EditPatient(int id, [FromBody] Patient updatedPatient)
        {
            var patient = patientRepository.GetById(id);
            if (patient == null)
            {
                return NotFound("Patient Not Valid");
            }

            patient.FirstName = updatedPatient.FirstName;
            patient.LastName = updatedPatient.LastName;
            patient.DateOfBirth = updatedPatient.DateOfBirth;
            patient.PhoneNumber = updatedPatient.PhoneNumber;
            patient.Gender = updatedPatient.Gender;
            patient.ImageImgData = updatedPatient.ImageImgData;
            patient.ImageFile = updatedPatient.ImageFile;
            patient.UserId = updatedPatient.UserId;
            patient.Points = updatedPatient.Points;
            patient.BloodType = updatedPatient.BloodType;

            patientRepository.Update(patient);
            patientRepository.Save();

            return NoContent();
        }

        // Delete a patient
        [HttpDelete("{id:int}")]
        public IActionResult DeletePatient(int id)
        {
            var patient = patientRepository.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }

            patientRepository.Delete(patient);
            patientRepository.Save();
            return NoContent();
        }

        // Get a patient by ID
        [HttpGet("{id:int}")]
        public IActionResult GetPatientById(int id)
        {
            var patient = patientRepository.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

    }
}

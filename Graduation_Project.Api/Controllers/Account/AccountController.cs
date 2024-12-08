using Graduation_Project.Api.DTO.Account;

namespace Graduation_Project.Api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RegistrationService registrationService;

        public AccountController(RegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        [HttpPost("DoctorRegister")]
        public async Task<IActionResult> DoctorRegister(DoctorDTO doctorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                await registrationService.RegisterAsync(doctorDTO);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during registration", Details = ex.Message });
            }
        }

        [HttpPost("PatientRegister")]
        public async Task<IActionResult> PatientRegister(PatientDTO patientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                await registrationService.RegisterAsync(patientDTO);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during registration", Details = ex.Message });
            }
        }

        [HttpPost("PharmacistRegister")]
        public async Task<IActionResult> PharmacistRegister(PharmacistDTO pharmacistDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                await registrationService.RegisterAsync(pharmacistDTO);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during registration", Details = ex.Message });
            }
        }

        [HttpPost("SecretaryRegister")]
        public async Task<IActionResult> SecretaryRegister(SecretaryDTO secretaryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                await registrationService.RegisterAsync(secretaryDTO);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during registration", Details = ex.Message });
            }
        }
    }
}

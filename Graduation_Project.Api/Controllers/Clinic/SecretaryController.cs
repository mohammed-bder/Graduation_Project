
using System.Data;
using Graduation_Project.Api.DTO.Clinic;
using Graduation_Project.Repository.Repository.Interfaces.Clinics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Graduation_Project.Api.Controllers.Clinic
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretaryController : ControllerBase
    {
        private readonly ISecretaryRepository secretaryRepository;

        public SecretaryController(ISecretaryRepository secretaryRepository)
        {
            this.secretaryRepository = secretaryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<SecDTO>? secretaries = secretaryRepository.GetAll_WithOut_Nav_prop();
            if (secretaries != null)
            {
                return Ok(secretaries);
            }
            return BadRequest();
        }

        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            SecDTO? secretary = secretaryRepository.GetById_WithOut_Nav_prop(Id);
            if (secretary != null)
            {
                return Ok(secretary);
            }
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Edit(SecDTO secDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            Secretary? secretaryFromDb = secretaryRepository.GetById(secDTO.Id);
            if (secretaryFromDb == null)
            {
                return BadRequest("Invalid secretary Id");
            }

            secretaryFromDb.FirstName = secDTO.FirstName;
            secretaryFromDb.LastName = secDTO.LastName;
            secretaryFromDb.DateOfBirth = secDTO.DateOfBirth;
            secretaryFromDb.PhoneNumber = secDTO.PhoneNumber;
            secretaryFromDb.Gender = secDTO.Gender;
            secretaryFromDb.ImageImgData = secDTO.ImageImgData;

            secretaryRepository.Update(secretaryFromDb);
            secretaryRepository.Save();

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            Secretary? secretary = secretaryRepository.GetById(Id);
            if (secretary == null)
            {
                return BadRequest();
            }

            secretaryRepository.Delete(secretary);
            secretaryRepository.Save();

            return Ok();
        }
    }
}

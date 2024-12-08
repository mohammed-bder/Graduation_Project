using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Models; // Assuming your Diagnosis model is in this namespace
using System.Collections.Generic;
using System.Linq;
using Graduation_Project.Repository.Repository.Interfaces.Patients;

namespace Graduation_Project.Api.Controllers.Patients
{
    [ApiController]
    [Route("api/[controller]")]
    public class AI_QuickDiagnosisController : ControllerBase
    {
        private readonly IAI_QuickDiagnosisRepository _repository;

        public AI_QuickDiagnosisController(IAI_QuickDiagnosisRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddDiagnosis([FromBody] AI_QuickDiagnosis aI_QuickDiagnosis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                aI_QuickDiagnosis.CreatedTime = DateTime.UtcNow;
                _repository.Add(aI_QuickDiagnosis);
                _repository.Save();

                return CreatedAtAction(nameof(GetDiagnosisById), new { id = aI_QuickDiagnosis.Id }, aI_QuickDiagnosis);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving diagnosis");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult EditDiagnosis(int id, [FromBody] AI_QuickDiagnosis updatedDiagnosis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDiagnosis = _repository.GetById(id);
            if (existingDiagnosis == null)
            {
                return NotFound();
            }

            existingDiagnosis.PatientId = updatedDiagnosis.PatientId;
            existingDiagnosis.CreatedTime = DateTime.UtcNow;
            existingDiagnosis.Symptoms = updatedDiagnosis.Symptoms;
            existingDiagnosis.Recommendations = updatedDiagnosis.Recommendations;

            _repository.Update(existingDiagnosis);
            _repository.Save();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteDiagnosis(int id)
        {
            var diagnosis = _repository.GetById(id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            _repository.Delete(diagnosis);
            _repository.Save();
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetDiagnosisById(int id)
        {
            var diagnosis = _repository.GetById(id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return Ok(diagnosis);
        }
    }

}

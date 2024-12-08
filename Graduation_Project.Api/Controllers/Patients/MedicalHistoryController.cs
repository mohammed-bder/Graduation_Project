using Graduation_Project.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Graduation_Project.Api.Controllers.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IGenericRepository<MedicalHistory> _repository;

        public MedicalHistoryController(IGenericRepository<MedicalHistory> repository)
        {
            _repository = repository;
        }

        // Add a new medical history
        [HttpPost]
        public async Task<ActionResult> AddMedicalHistory([FromBody] MedicalHistory medicalHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.AddAsync(medicalHistory);
                await _repository.SaveAsync();
                return CreatedAtAction(nameof(GetMedicalHistoryByIdAsync), new { id = medicalHistory.Id }, medicalHistory);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving the medical history.");
            }
        }

        // Edit an existing medical history
        [HttpPut("{id:int}")]
        public IActionResult EditMedicalHistory(int id, [FromBody] MedicalHistory updatedMedicalHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalHistory = _repository.GetByIdAsync(id);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            medicalHistory.Name = updatedMedicalHistory.Name;
            medicalHistory.Details = updatedMedicalHistory.Details;
            medicalHistory.Date = updatedMedicalHistory.Date;
            medicalHistory.PatientId = updatedMedicalHistory.PatientId;
            medicalHistory.MedicalCategoryId = updatedMedicalHistory.MedicalCategoryId;

            try
            {
                _repository.Update(medicalHistory);
                _repository.Save();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the medical history.");
            }
        }

        // Delete a medical history
        [HttpDelete("{id:int}")]
        public IActionResult DeleteMedicalHistory(int id)
        {
            var medicalHistory = _repository.GetById(id);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            try
            {
                _repository.Delete(medicalHistory);
                _repository.Save();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the medical history.");
            }
        }

        // Get a medical history by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMedicalHistoryByIdAsync(int id)
        {
            var medicalHistory = await _repository.GetByIdAsync(id);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            return Ok(medicalHistory);
        }

        // Get all medical histories
        [HttpGet]
        public async Task<IActionResult> GetAllMedicalHistoriesAsync()
        {
            try
            {
                var medicalHistories = await _repository.GetAllAsync();
                return Ok(medicalHistories);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving medical histories.");
            }
        }
    }
}

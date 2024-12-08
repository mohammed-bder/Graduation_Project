using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Models; // Assuming your MedicalCategory model is in this namespace
using System.Collections.Generic;
using System.Threading.Tasks;
using Graduation_Project.Repository.Repository.Interfaces.Patients;

namespace Graduation_Project.Api.Controllers.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCategoryController : ControllerBase
    {
        private readonly IMedicalCategoryRepository _repository;

        public MedicalCategoryController(IMedicalCategoryRepository repository)
        {
            _repository = repository;
        }

        // Add a new medical category
        [HttpPost]
        public IActionResult AddMedicalCategory([FromBody] MedicalCategory medicalCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _repository.Add(medicalCategory);
                _repository.Save();

                return CreatedAtAction(nameof(GetMedicalCategoryById), new { id = medicalCategory.Id }, medicalCategory);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving the medical category.");
            }
        }

        // Edit an existing medical category
        [HttpPut("{id:int}")]
        public IActionResult EditMedicalCategory(int id, [FromBody] MedicalCategory updatedMedicalCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalCategory = _repository.GetById(id);
            if (medicalCategory == null)
            {
                return NotFound();
            }

            medicalCategory.Name = updatedMedicalCategory.Name;

            try
            {
                _repository.Update(medicalCategory);
                _repository.Save();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the medical category.");
            }
        }

        // Delete a medical category
        [HttpDelete("{id:int}")]
        public IActionResult DeleteMedicalCategory(int id)
        {
            var medicalCategory = _repository.GetById(id);
            if (medicalCategory == null)
            {
                return NotFound();
            }

            try
            {
                _repository.Delete(medicalCategory);
                _repository.Save();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the medical category.");
            }
        }

        // Get a medical category by ID
        [HttpGet("{id:int}")]
        public IActionResult GetMedicalCategoryById(int id)
        {
            var medicalCategory = _repository.GetById(id);
            if (medicalCategory == null)
            {
                return NotFound();
            }

            return Ok(medicalCategory);
        }

        // Get all medical categories
        [HttpGet]
        public IActionResult GetAllMedicalCategories()
        {
            try
            {
                var medicalCategories = _repository.GetAll();
                return Ok(medicalCategories);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving medical categories.");
            }
        }
    }
}

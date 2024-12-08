using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Models; // Assuming your RadiologyReport model is in this namespace
using System.Collections.Generic;
using Graduation_Project.Repository.Repository.Repos.Patients;

namespace Graduation_Project.Api.Controllers.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    public class RadiologyReportController : ControllerBase
    {
        private readonly RadiologyReportRepository radiologyReportRepository;

        public RadiologyReportController(RadiologyReportRepository radiologyReportRepository)
        {
            this.radiologyReportRepository = radiologyReportRepository;
        }

        // Add a new radiology report
        [HttpPost]
        public IActionResult AddRadiologyReport([FromBody] RadiologyReport radiologyReport)
        {
            radiologyReportRepository.Add(radiologyReport);
            radiologyReportRepository.Save();
            return CreatedAtAction(nameof(GetRadiologyReportById), new { id = radiologyReport.Id }, radiologyReport);
        }

        // Edit an existing radiology report
        [HttpPut("{id:int}")]
        public IActionResult EditRadiologyReport(int id, [FromBody] RadiologyReport updatedRadiologyReport)
        {
            var radiologyReport = radiologyReportRepository.GetById(id);
            if (radiologyReport == null)
            {
                return NotFound();
            }

            radiologyReport.Diagnosis = updatedRadiologyReport.Diagnosis;
            radiologyReport.ImageData = updatedRadiologyReport.ImageData;
            radiologyReport.AIAnalysis = updatedRadiologyReport.AIAnalysis;
            radiologyReport.CreatedDate = updatedRadiologyReport.CreatedDate;
            radiologyReport.PatientId = updatedRadiologyReport.PatientId;

            radiologyReportRepository.Update(radiologyReport);
            radiologyReportRepository.Save();

            return NoContent();
        }

        // Delete a radiology report
        [HttpDelete("{id:int}")]
        public IActionResult DeleteRadiologyReport(int id)
        {
            var radiologyReport = radiologyReportRepository.GetById(id);
            if (radiologyReport == null)
            {
                return NotFound();
            }

            radiologyReportRepository.Delete(radiologyReport);
            radiologyReportRepository.Save();
            return NoContent();
        }

        // Get a radiology report by ID
        [HttpGet("{id:int}")]
        public IActionResult GetRadiologyReportById(int id)
        {
            var radiologyReport = radiologyReportRepository.GetById(id);
            if (radiologyReport == null)
            {
                return NotFound();
            }

            return Ok(radiologyReport);
        }

        // Get all radiology reports
        [HttpGet]
        public IActionResult GetAllRadiologyReports()
        {
            var radiologyReports = radiologyReportRepository.GetAll();
            return Ok(radiologyReports);
        }
    }
}


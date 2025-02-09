using Graduation_Project.Api.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{
    public class MetaDataController : BaseApiController
    {
        /********************** Get All Doctor Degrees **********************/
        [HttpGet("DoctorDegrees")]
        public IActionResult GetDoctorDegrees() 
        {
            string[] degrees = Enum.GetNames(typeof(DoctorDegree)); // ["Bachelor","Master","Doctorate","Other"]

            var response = new MetaDataDto
            {
                TotalCount = degrees.Length,
                Description = "List of doctor degrees.",
                Data = degrees
            };

            return Ok(response);
        }

        /********************** Get All genders **********************/
        [HttpGet("Gender")]
        public IActionResult Gender()
        {
            string[] degrees = Enum.GetNames(typeof(Gender));

            var response = new MetaDataDto
            {
                TotalCount = degrees.Length,
                Description = "List of genders.",
                Data = degrees
            };

            return Ok(response);
        }

        /********************** Get All Bold Type **********************/
        [HttpGet("BloodTypes")]
        public IActionResult BloodTypes()
        {
            string[] degrees = Enum.GetNames(typeof(BloodType)); 

            var response = new MetaDataDto
            {
                TotalCount = degrees.Length,
                Description = "List of blood types.",
                Data = degrees
            };

            return Ok(response);
        }

        /********************** Get All Clinic Type **********************/
        [HttpGet("ClinicTypes")]
        public IActionResult ClinicTypes()
        {
            string[] degrees = Enum.GetNames(typeof(ClinicType));

            var response = new MetaDataDto
            {
                TotalCount = degrees.Length,
                Description = "List of Clinic types.",
                Data = degrees
            };

            return Ok(response);
        }
    }
}

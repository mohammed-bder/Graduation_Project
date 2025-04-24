using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.DTO.Pharmacies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.PharmacyControllers
{
    public class PharmacyController : BaseApiController
    {
        [HttpGet]
        public ActionResult FindNearestPharmacies(PatientLocationWithMedicinesDto patientLocationWithMedicinesDto)
        {
            return Ok();
        }

    }
}

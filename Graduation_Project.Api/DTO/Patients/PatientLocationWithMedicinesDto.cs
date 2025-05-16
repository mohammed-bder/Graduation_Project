using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Patients
{
    public class PatientLocationWithMedicinesDto
    {
        [Required(ErrorMessage = "Latitude is required.")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required.")]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double? Longtude { get; set; }

        [Required(ErrorMessage = "You Must Insert Medicines")]
        [MinLength(1, ErrorMessage = "The list must contain at least one medicine")]
        public List<int> Medicines { get; set; }
    }
}

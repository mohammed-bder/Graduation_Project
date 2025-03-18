using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Api.DTO.Shared
{
    public class PrescriptionFromUserDto
    {
        [StringLength(1500, ErrorMessage = "Comment length can't be more than 1500 characters.")]
        public string? Diagnoses { get; set; }
        //public DateTime IssuedDate { get; set; }

        //public int DoctorId { get; set; }
        [Required]
        [ExistingId<Patient>]
        public int PatientId { get; set; }

        public ICollection<MedicinePrescriptionDto> MedicinePrescriptions { get; set; }

        public ICollection<PrescriptionImageDTO>? PrescriptionImages { get; set; }
    }
}

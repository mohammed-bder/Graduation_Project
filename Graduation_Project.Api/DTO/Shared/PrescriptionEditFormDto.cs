using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Shared
{
    public class PrescriptionEditFormDto
    {
        public int Id { get; set; }

        [StringLength(1500, ErrorMessage = "Comment length can't be more than 1500 characters.")]
        public string? Diagnoses { get; set; }
        public ICollection<MedicinePrescriptionDto> MedicinePrescriptions { get; set; }

        public ICollection<PrescriptionImageDTO> prescriptionImages { get; set; }
    }
}

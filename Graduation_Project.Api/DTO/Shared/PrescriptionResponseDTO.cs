using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Shared
{
    public class PrescriptionResponseDTO
    {
        public string PatientName { get; set; }
        public int PatientAge { get; set; }

        public string DoctorName { get; set; }

        public string Diagnoses { get; set; }

        public string IssuedDate { get; set; }
        public int PatientId { get; set; }

        public ICollection<MedicinePrescriptionResponseDTO> MedicinePrescriptions { get; set; }

        public ICollection<PrescriptionImageDTO> prescriptionImages { get; set; }
    }
}

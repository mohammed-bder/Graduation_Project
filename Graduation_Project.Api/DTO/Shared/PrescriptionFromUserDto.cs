using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Api.DTO.Shared
{
    public class PrescriptionFromUserDto
    {
        public string? Diagnoses { get; set; }
        //public DateTime IssuedDate { get; set; }

        //public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public ICollection<MedicinePrescriptionDto> MedicinePrescriptions { get; set; }
    }
}

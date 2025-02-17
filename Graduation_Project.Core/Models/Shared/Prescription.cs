namespace Graduation_Project.Core.Models.Shared
{
    public class Prescription : BaseEntity
    {
        public string? Diagnoses { get; set; }
        public DateTime IssuedDate { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public ICollection<MedicinePrescription>? MedicinePrescriptions { get; set; }
    }
}

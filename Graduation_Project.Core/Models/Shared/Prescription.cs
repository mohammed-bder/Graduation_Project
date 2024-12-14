namespace Graduation_Project.Core.Models.Shared
{
    public class Prescription : BaseEntity
    {
        public string MedicationDetails { get; set; }
        public string Dosage { get; set; }
        public DateTime IssuedDate { get; set; }
        public int Duration { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}

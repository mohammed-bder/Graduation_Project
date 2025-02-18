namespace Graduation_Project.Core.Models.Shared
{
    public class Feedback : BaseEntity
    {
        public string Comment { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; } 
        [ForeignKey("PatientId")]
        public Patient patient { get; set; }
    }
}

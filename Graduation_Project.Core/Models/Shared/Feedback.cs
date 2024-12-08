namespace Graduation_Project.Core.Models.Shared
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }


        public int PatientId { get; set; } // me
        [ForeignKey("PatientId")]
        public Patient patient { get; set; }
    }
}

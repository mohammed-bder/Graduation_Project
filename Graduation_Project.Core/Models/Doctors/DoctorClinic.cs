namespace Graduation_Project.Core.Models.Doctors
{
    public class DoctorClinic : BaseEntity
    {

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }


        [Required]
        [RegularExpression("Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday",
        ErrorMessage = "Invalid day of the week.")]
        public string DayOfWeek { get; set; } // e.g., "Monday"

        [Required]
        [DataType(DataType.Time)]
        public DateTime  StartDate  { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime  EndDate  { get; set; }
    }
}

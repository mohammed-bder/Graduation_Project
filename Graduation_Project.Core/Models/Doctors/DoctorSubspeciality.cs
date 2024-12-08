namespace Graduation_Project.Core.Models.Doctors
{
    public class DoctorSubspeciality
    {
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("SubSpecialities")]
        public int SubSpecialitiesId { get; set; }
        public SubSpecialities SubSpecialities { get; set; }
    }
}

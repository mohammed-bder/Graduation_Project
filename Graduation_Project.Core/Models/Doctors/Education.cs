namespace Graduation_Project.Core.Models.Doctors
{
    public class Education : BaseEntity
    {
        public DoctorDegree Degree { get; set; }
        public string Institution { get; set; }
        public string Certifications { get; set; }
        public string Fellowships { get; set; }
        /* ----------------- Relationships ----------------- */

        // (M Education ==> 1 Doctor)
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }
}

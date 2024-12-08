namespace Graduation_Project.Core.Models.Doctors
{
    public class Specialty
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Specialty name is required.")]
        [StringLength(50, ErrorMessage = "Specialty name cannot exceed 50 characters.")]
        public string Name { get; set; }

        /* ----------------- Relationships ----------------- */

        // (1 Specialty ==> M SubSpecialities)
        public ICollection<SubSpecialities> SubSpecialities { get; set; }

        // (1 Specialty ==> M Doctor)
        public ICollection<Doctor> Doctors { get; set; }
    }
}

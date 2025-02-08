namespace Graduation_Project.Core.Models.Doctors
{
    public class SubSpecialities : BaseEntity
    {
        [Required(ErrorMessage = "Subspeciality name is required.")]
        [StringLength(50, ErrorMessage = "Subspeciality name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        /* ----------------- Relationships ----------------- */

        // (M SubSpecialities ==> M Doctor)
        public ICollection<DoctorSubspeciality>? DoctorSubspeciality { get; set; }

        // (M SubSpecialities ==> 1 Specialty)
        public  int SpecialtyId { get; set; }
        public  Specialty Specialty { get; set; }
    }
}

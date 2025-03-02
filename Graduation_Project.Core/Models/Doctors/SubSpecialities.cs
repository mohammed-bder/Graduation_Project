namespace Graduation_Project.Core.Models.Doctors
{
    public class SubSpecialities : BaseEntity
    {
        [Required(ErrorMessage = "Subspeciality name is required.")]
        public string Name_ar { get; set; }
        [Required(ErrorMessage = "Subspeciality name is required.")]
        public string Name_en { get; set; }

        /* ----------------- Relationships ----------------- */

        // (M SubSpecialities ==> M Doctor)
        public ICollection<DoctorSubspeciality>? DoctorSubspeciality { get; set; }

        // (M SubSpecialities ==> 1 Specialty)
        public  int SpecialtyId { get; set; }
        public  Specialty Specialty { get; set; }
    }
}

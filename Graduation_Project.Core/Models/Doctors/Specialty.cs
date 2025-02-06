using System.Text.Json.Serialization;

namespace Graduation_Project.Core.Models.Doctors
{
    public class Specialty : BaseEntity
    {
        [Required(ErrorMessage = "Specialty name is required.")]
        [StringLength(100, ErrorMessage = "Specialty name cannot exceed 100 characters.")]
        public required string Name { get; set; }

        /* ----------------- Relationships ----------------- */

        // (1 Specialty ==> M SubSpecialities)  but some specialties don't have subspecialties
        [JsonPropertyName("SubSpecialties")] // Map JSON property to this C# property
        public ICollection<SubSpecialities>? SubSpecialities { get; set; }

        // (1 Specialty ==> M Doctor) but some specialties don't have doctors(not yet)
        public ICollection<Doctor>? Doctors { get; set; }
    }
}

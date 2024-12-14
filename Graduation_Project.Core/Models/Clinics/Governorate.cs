namespace Graduation_Project.Core.Models.Clinics
{
    public class Governorate : BaseEntity
    {
        [Required(ErrorMessage = "Governorate name is required. Please provide a name.")]
        public string Name { get; set; }

        public List<Region> regions { get; set; }

    }
}

namespace Graduation_Project.Core.Models.Clinics
{
    public class Region : BaseEntity
    {
        [Required(ErrorMessage = "Region name is required. Please provide a name.")]
        public string Name { get; set; }

        public int governorateId { get; set; }
        [ForeignKey("governorateId")]
        public Governorate governorate { get; set; }

        public List<Clinic> clinics { get; set; }
    }
}

namespace Graduation_Project.Api.DTO.Clinics
{
    public class ClinicEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } // Name of the clinic
        public string? PictureUrl { get; set; }
        public string? LocationLink { get; set; } //  locationLink of the clinic
        public string? Location { get; set; }     //  Address or location of the clinic
        public ClinicType? Type { get; set; }
        // Relationships
        public int RegionId { get; set; }

        public int GovernorateId { get; set; }
    }
}

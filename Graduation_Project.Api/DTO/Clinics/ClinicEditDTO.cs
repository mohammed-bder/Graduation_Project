namespace Graduation_Project.Api.DTO.Clinics
{
    public class ClinicEditDTO
    {

        public string Name { get; set; } // Name of the clinic
        public string? PictureUrl { get; set; }
        public double Latitude { get; set; }  // Latitude coordinate
        public double Longitude { get; set; }  // Longitude coordinate
        public string? Address { get; set; }     //  Address or location of the clinic

        public ClinicType? Type { get; set; }
        // Relationships
        public int RegionId { get; set; }

        public int GovernorateId { get; set; }
    }
}

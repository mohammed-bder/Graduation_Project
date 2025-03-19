namespace Graduation_Project.Api.DTO.Clinics
{
    public class ClinicAboutDto
    {
        public string Name { get; set; } // Name of the clinic
        public string? PictureUrl { get; set; }
        public string? LocationLink { get; set; } //  locationLink of the clinic
        //public double Latitude { get; set; }  // Latitude coordinate
        //public double Longitude { get; set; }  // Longitude coordinate

        public string? Address { get; set; }     //  Address or location of the clinic


        public ICollection<ContactNumberDTO>? contactNumbers { get; set; }
    }
}

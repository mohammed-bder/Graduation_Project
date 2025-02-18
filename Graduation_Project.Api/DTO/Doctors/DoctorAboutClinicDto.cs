namespace Graduation_Project.Api.DTO.Doctors
{
    public class DoctorAboutClinicDto
    {
        public string Name { get; set; } // Name of the clinic
        public string? PictureUrl { get; set; }
        public string? LocationLink { get; set; } //  locationLink of the clinic
        public string? Location { get; set; }     //  Address or location of the clinic
    }
}

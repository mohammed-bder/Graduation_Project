namespace Graduation_Project.Api.DTO.Doctors
{
    public class DoctorAboutDto 
    {
        //******************************* Doctor *******************************/
        public string? Description { get; set; }
        public IEnumerable<string>? DoctorSubspeciality { get; set; }
        //******************************* Education *******************************/
        public DoctorDegree? Degree { get; set; }
        public string? Institution { get; set; }
        public string? Certifications { get; set; }
        public string? Fellowships { get; set; }
        //******************************* Clinic *******************************/
        public string Name { get; set; } // Name of the clinic
        public ICollection<string>? PictureUrls { get; set; }
        public string? LocationLink { get; set; } //  locationLink of the clinic
        public string? Location { get; set; }     //  Address or location of the clinic
        //public DoctorAboutClinicDto DoctorClinics { get; set; }
    }
}

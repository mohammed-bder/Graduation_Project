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
        public List<DoctorAboutClinicDto> DoctorClinics { get; set; }
    }
}

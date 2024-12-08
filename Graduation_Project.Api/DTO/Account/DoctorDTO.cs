namespace Graduation_Project.Api.DTO.Account
{
    public class DoctorDTO : RegisterDTO
    {
        public int SpecialtyId { get; set; }
        public decimal ConsultationFees { get; set; } = decimal.Zero;
        public string? MedicalLicence { get; set; }
    }
}

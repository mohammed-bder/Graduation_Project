namespace Graduation_Project.Api.DTO.Patients
{
    public class MedicalHistoryInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public DateTime Date { get; set; }
        public string? MedicalImage { get; set; }
    }
}

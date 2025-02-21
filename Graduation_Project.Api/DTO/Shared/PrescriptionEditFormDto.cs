namespace Graduation_Project.Api.DTO.Shared
{
    public class PrescriptionEditFormDto
    {
        public string? Diagnoses { get; set; }
        public ICollection<MedicinePrescriptionDto> MedicinePrescriptions { get; set; }
    }
}

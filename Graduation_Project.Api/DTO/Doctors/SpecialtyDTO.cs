namespace Graduation_Project.Api.DTO.Doctors
{
    public class SpecialtyDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Specialty Name is required.")]
        public string Name { get; set; }
    }
}

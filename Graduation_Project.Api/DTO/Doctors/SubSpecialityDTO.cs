namespace Graduation_Project.Api.DTO.Doctors
{
    public class SubSpecialityDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Subspeciality name is required.")]
        [StringLength(50, ErrorMessage = "Subspeciality name cannot exceed 50 characters.")]
        public string Name { get; set; }

    }
}

namespace Graduation_Project.Api.DTO
{
    public class SpecialityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<SubSpecialityDTO> SubSpecialities { get; set; }
    }
}

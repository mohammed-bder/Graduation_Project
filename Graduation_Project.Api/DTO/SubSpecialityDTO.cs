namespace Graduation_Project.Api.DTO
{
    public class SubSpecialityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // (M SubSpecialities ==> 1 Specialty)
        public int SpecialtyId { get; set; }
        public string Specialty { get; set; }
    }
}

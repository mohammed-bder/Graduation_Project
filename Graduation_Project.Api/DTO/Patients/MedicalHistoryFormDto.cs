using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Graduation_Project.Api.DTO.Patients
{
    public class MedicalHistoryFormDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Details cannot exceed 500 characters.")]
        public string? Details { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MedicalImage { get; set; }

        [JsonIgnore]
        public IFormFile? PictureFile { get; set; }

        [Required(ErrorMessage = "Medical Category ID is required.")]
        [ExistingId<MedicalCategory>]
        public int MedicalCategoryId { get; set; }
    }
}

using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Api.DTO.Shared
{
    public class MedicinePrescriptionDto
    {
        [Required]
        [ExistingId<Medicine>]
        public int MedicineId { get; set; }

        [StringLength(500, ErrorMessage = "Comment length can't be more than 500 characters.")]
        public string Details { get; set; }
    }
}

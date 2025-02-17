using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graduation_Project.Core.Models.Doctors;

namespace Graduation_Project.Api.DTO.Patients
{
    public class FeedbackDto
    {
        [Required]
        [StringLength(500, ErrorMessage = "Comment length can't be more than 500 characters.")]
        public string Comment { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Score must be between 1 and 5.")]
        public int Score { get; set; } = 0;

        [Required]
        [ExistingId<Doctor>]
        public int DoctorId { get; set; }
    }
}

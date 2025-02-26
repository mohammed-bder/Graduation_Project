using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Shared
{
    public class BookAppointmentDto
    {
        [Required]
        [ExistingId<Doctor>]
        public int doctorId { get; set; }
        [Required]
        [ExistingId<Patient>]
        public int patientId { get; set; }
        [Required]
        public DateOnly date { get; set; }
        [Required]
        public TimeOnly time { get; set; }
    }

}

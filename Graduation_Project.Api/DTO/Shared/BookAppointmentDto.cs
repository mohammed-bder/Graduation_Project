using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Shared
{
    public class BookAppointmentDto
    {
        [Required]
        [ExistingId<Doctor>]
        public int DoctorId { get; set; }
        [Required]
        public DateOnly AppointmentDate { get; set; }
        [Required]
        public TimeOnly AppointmentTime { get; set; }
    }

}

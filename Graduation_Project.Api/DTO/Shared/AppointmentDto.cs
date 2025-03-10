using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Shared
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [ForeignKey("Patient")]
        //public int PatientId { get; set; } // Assuming a Patient entity exists
        public string PatientName { get; set; }

        public DateOnly AppointmentDate { get; set; } // Stores the exact date of the appointment

        public TimeOnly AppointmentTime { get; set; } // Stores the booked slot time

        public AppointmentStatus Status { get; set; } // Track the status of the appointment (Pending, Confirmed, etc.)

    }
}

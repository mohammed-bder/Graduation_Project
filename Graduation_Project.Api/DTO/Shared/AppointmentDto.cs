using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Shared
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Status { get; set; }
        public string AppointmentTime { get; set; } // Time in "HH:mm:ss" format
        public string AppointmentDate { get; set; } // Stores the exact date of the appointment
        public Gender Gender { get; set; }
        public string Age { get; set; }
    }
}

namespace Graduation_Project.Api.DTO.Shared
{
    public class AppointmentForPatientDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Status { get; set; }
        public string AppointmentTime { get; set; } // Time in "HH:mm:ss" format
        public string AppointmentDate { get; set; } // Stores the exact date of the appointment
    }
}

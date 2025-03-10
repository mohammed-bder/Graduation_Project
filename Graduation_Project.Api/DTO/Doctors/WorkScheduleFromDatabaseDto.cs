namespace Graduation_Project.Api.DTO.Doctors
{
    public class WorkScheduleFromDatabaseDto
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; } // e.g., Monday, Tuesday...
        public TimeOnly StartTime { get; set; } // e.g., 09:00 AM
        public TimeOnly EndTime { get; set; } // e.g., 05:00 PM
    }
}

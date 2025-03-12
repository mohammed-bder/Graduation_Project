using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Doctors
{
    public class ScheduleExceptionFromUserDto
    {
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        [DataType(DataType.Time)]
        public TimeOnly? StartTime { get; set; } // e.g., 09:00 AM
        [DataType(DataType.Time)]
        public TimeOnly? EndTime { get; set; } // e.g., 05:00 PM
        public bool IsAvailable { get; set; } // False means the doctor is unavailable
    }
}

using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Doctors
{
    public class WorkScheduleFromUserDto
    {
        [ExistingId<Doctor>]
        public int doctorId { get; set; }

        [ValidEnumValue<DayOfWeek>(ErrorMessage = "Invalid value for DayOfWeek.")]
        public DayOfWeek Day { get; set; } // e.g., Monday, Tuesday...

        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; } // e.g., 09:00 AM

        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; } // e.g., 05:00 PM
    }
}

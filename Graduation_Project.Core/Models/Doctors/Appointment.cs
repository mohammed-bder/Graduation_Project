using Graduation_Project.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Models.Doctors
{
    public class Appointment : BaseEntity
    {
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; } // Assuming a Patient entity exists
        public Patient Patient { get; set; }

        [Required]
        public DateOnly AppointmentDate { get; set; } // Stores the exact date of the appointment

        [Required]
        public TimeOnly AppointmentTime { get; set; } // Stores the booked slot time
    }

}

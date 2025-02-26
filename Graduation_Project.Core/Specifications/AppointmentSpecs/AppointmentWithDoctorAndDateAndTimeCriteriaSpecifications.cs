using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.AppointmentSpecs
{
    public class AppointmentWithDoctorAndDateAndTimeCriteriaSpecifications : BaseSpecifications<Appointment>
    {
        public AppointmentWithDoctorAndDateAndTimeCriteriaSpecifications(int doctorId, DateOnly date, TimeOnly time)
            : base(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate == date &&
                a.AppointmentTime == time
            )
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.AppointmentSpecs
{
    public class AppointmentSpecifications : BaseSpecifications<Appointment>
    {
        public AppointmentSpecifications(int doctorId, DateOnly fromDate)
            : base(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate >= fromDate
            )
        {

        }
    }
}

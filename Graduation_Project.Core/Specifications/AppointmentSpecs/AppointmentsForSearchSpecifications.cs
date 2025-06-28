using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.AppointmentSpecs
{

    /*
     Purpose: Fetches upcoming appointments for a doctor within the next month that are either confirmed or pending.

     Includes:
     
     -Patient
     
     Ordering: By date and then time.
     
     Relation: Helps secretaries or doctors view their upcoming appointments.
     
     */

    public class AppointmentsForSearchSpecifications : BaseSpecifications<Appointment>
    {
        public AppointmentsForSearchSpecifications(int doctorId, DateOnly startDate)
            : base(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate >= startDate &&
                a.AppointmentDate < startDate.AddMonths(1) && // Fetch appointments within 1 month
                (a.Status == AppointmentStatus.Confirmed || a.Status == AppointmentStatus.Pending) // Only booked appointments
            )
        {
            Includes.Add(a => a.Patient); // Include patient details
            AddOrderBy(a => a.AppointmentDate); // Order by date
            AddThenOrderBy(a => a.AppointmentTime); // Then order by time
        }
    }
}

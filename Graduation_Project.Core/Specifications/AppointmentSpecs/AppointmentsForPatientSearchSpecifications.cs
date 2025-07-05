using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Graduation_Project.Core.Specifications.AppointmentSpecs
{
    public class AppointmentsForPatientSearchSpecifications: BaseSpecifications<Appointment>
    {
        public AppointmentsForPatientSearchSpecifications(int patientId, DateOnly date)
            : base(a =>
                a.PatientId == patientId &&
                a.AppointmentDate == date
            )
        {
            Includes.Add(a => a.Doctor); // ✅ Includes Doctor
            ThenIncludes.Add(q => q.Include(d => d.Doctor.Specialty)); // ✅ Includes Specialty inside Doctor
            AddOrderBy(a => a.AppointmentDate); // Order by date
            AddThenOrderBy(a => a.AppointmentTime); // Then order by time
        }
    }
}

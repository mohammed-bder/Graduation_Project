using Graduation_Project.Core.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.AppointmentSpecs
{
    public class AppointmentByPatientDoctorDateSpec : BaseSpecifications<Appointment>
    {
        public AppointmentByPatientDoctorDateSpec(int patientId, int doctorId, DateOnly date)
            : base(a => a.PatientId == patientId
                 && a.DoctorId == doctorId
                 && a.AppointmentDate == date
            )
        {

        }
    }
}

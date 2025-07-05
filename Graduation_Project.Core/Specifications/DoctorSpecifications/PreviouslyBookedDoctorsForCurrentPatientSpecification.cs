using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class PreviouslyBookedDoctorsForCurrentPatientSpecification : BaseSpecifications<Appointment>
    {

        public PreviouslyBookedDoctorsForCurrentPatientSpecification(int patientId) : base(a => a.PatientId == patientId)
        {
            Includes.Add(a => a.Doctor);
        }

        public PreviouslyBookedDoctorsForCurrentPatientSpecification(int patientId, FavrouiteDoctorSpecParams favrouiteDoctorSpecParams) : base(a => a.PatientId == patientId)
        {
            
            ThenIncludes.Add(a => a.Include(a => a.Doctor).ThenInclude(d => d.WorkSchedules));
            ThenIncludes.Add(a => a.Include(a => a.Doctor).ThenInclude(d => d.ScheduleExceptions));


            ThenIncludes.Add(a => a.Include(a => a.Doctor).ThenInclude(d => d.Specialty));
            ThenIncludes.Add(a => a.Include(a => a.Doctor).ThenInclude(d => d.Clinic).ThenInclude(c => c.Region));
            ThenIncludes.Add(a => a.Include(a => a.Doctor).ThenInclude(d => d.Clinic).ThenInclude(c => c.Governorate));



            ApplyPagination((favrouiteDoctorSpecParams.PageIndex - 1) * favrouiteDoctorSpecParams.PageSize, favrouiteDoctorSpecParams.PageSize);
        }
    }
}

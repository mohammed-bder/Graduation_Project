using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.SecretarySpecifications
{
    public class PatientWithAppointmentForTodaySpecification : BaseSpecifications<Patient>
    {
        public PatientWithAppointmentForTodaySpecification( int PatientId)
            : base(p => p.Id == PatientId)
        {
            Includes.Add(p => p.Appointments);
           
        }
    }

}

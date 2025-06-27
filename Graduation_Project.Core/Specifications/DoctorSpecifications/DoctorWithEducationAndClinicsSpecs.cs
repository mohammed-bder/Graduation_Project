using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithEducationAndClinicsSpecs : BaseSpecifications<Doctor>
    {
        public DoctorWithEducationAndClinicsSpecs(int id) : base (d => d.Id == id) // where criteria
        {
            Includes.Add(d => d.Clinic);
            Includes.Add(d => d.Education);
            Includes.Add(d => d.DoctorSubspeciality);
            ThenIncludes.Add(d => d.Include(d => d.Specialty).ThenInclude(s => s.SubSpecialities));
            ThenIncludes.Add(d => d.Include(d => d.Clinic).ThenInclude(c => c.ClinicPictures));

                   

            //ThenIncludes.Add(d => d.Include(d => d.Clinic)
            //                       .Include(d => d.Education)
            //                       .Include(d => d.DoctorSubspeciality)
            //                       .ThenInclude(s => s.SubSpecialities));

        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.ClinicsSpecifications
{
    public class ClinicByDocIdWithAllDataSpecification : BaseSpecifications<Clinic>
    {

        public ClinicByDocIdWithAllDataSpecification() : base()
        {
            //Includes.Add(c => c.Region);
            //Includes.Add(C => C.ContactNumbers);

            ThenIncludes.Add(
                C => C.Include(c => c.Region)
                .ThenInclude(r => r.governorate)
                .Include(c => c.ContactNumbers)
                );
        }


        public ClinicByDocIdWithAllDataSpecification(int doctorId) : base( c => c.DoctorId == doctorId)
        {
            //Includes.Add(c => c.Region);
            //Includes.Add(C => C.ContactNumbers);

            ThenIncludes.Add(
                C => C.Include(c => c.Region)
                .ThenInclude(r => r.governorate)
                .Include(c => c.ContactNumbers)
                );
        }
    }
}

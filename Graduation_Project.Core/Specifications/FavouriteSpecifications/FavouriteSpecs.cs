using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.Specifications.DoctorSpecifications;

namespace Graduation_Project.Core.Specifications.FavouriteSpecifications
{
    public class FavouriteSpecs : BaseSpecifications<Favorite>
    {
        public FavouriteSpecs(int doctorId,int patientId) 
                            : base (f => f.PatientId == patientId && f.DoctorId == doctorId)
        {
            
        }
        public FavouriteSpecs(int patientId) : base(f => f.PatientId == patientId)
        {

        }

        public FavouriteSpecs(int patientId, FavrouiteDoctorSpecParams favrouiteDoctorSpecParams) : base (f => f.PatientId == patientId)
        {
            Includes.Add(f => f.Doctor);
            ApplyPagination((favrouiteDoctorSpecParams.PageIndex - 1) * favrouiteDoctorSpecParams.PageSize, favrouiteDoctorSpecParams.PageSize);
        }

    }
}

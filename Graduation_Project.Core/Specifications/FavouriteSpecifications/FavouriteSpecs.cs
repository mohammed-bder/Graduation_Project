using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.FavouriteSpecifications
{
    public class FavouriteSpecs : BaseSpecifications<Favorite>
    {
        public FavouriteSpecs(int doctorId,int patientId) 
                            : base (f => f.PatientId == patientId && f.DoctorId == doctorId)
        {
            
        }
    }
}

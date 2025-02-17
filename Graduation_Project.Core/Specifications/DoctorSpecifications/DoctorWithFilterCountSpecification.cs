using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithFilterCountSpecification: BaseSpecifications<Doctor>
    {
        public DoctorWithFilterCountSpecification(DoctorSpecParams specParams)
            : base(d =>

                (
                    string.IsNullOrEmpty(specParams.Search) ||
                    (
                        d.FirstName.ToLower().Contains(specParams.FirstNameSearch) &&
                        (specParams.LastNameSearch == null || d.LastName.ToLower().Contains(specParams.LastNameSearch))
                    )
                ) &&
                (!specParams.MaxPrice.HasValue || d.ConsultationFees <= specParams.MaxPrice) &&
                (!specParams.Gender.HasValue || d.Gender == specParams.Gender) &&
                (!specParams.SubSpecialtyId.HasValue || d.DoctorSubspeciality.Any(ds => ds.SubSpecialitiesId == specParams.SubSpecialtyId.Value)) &&
                (!specParams.SpecialtyId.HasValue || d.SpecialtyId == specParams.SpecialtyId)
            )
        {
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.Specifications.DoctorSpecifications
{
    public class SortingDoctorWithSpecificaiton : BaseSpecifications<Doctor>
    {
        public SortingDoctorWithSpecificaiton(string? sort) : base()
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Rating":
                        AddOrderByDescending(d => d.Rating);
                        break;
                    case "consultionFee":
                        AddOrderBy(d => d.ConsultationFees);
                        break;
                    case "consultionFeeDesc":
                        AddOrderByDescending(d => d.ConsultationFees);
                        break;
                    default:
                        AddOrderByDescending(d => d.Rating);
                        break;
                }
            }
            else
            {
                AddOrderByDescending(p => p.Rating);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IPharmacyService
    {
        object GetNearestPharmacies(double Patient_Longitude, double Patient_Latitude, IReadOnlyList<Pharmacy> pharmacies);
    }
}

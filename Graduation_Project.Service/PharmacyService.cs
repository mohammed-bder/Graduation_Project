using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Service.HelperModels;

namespace Graduation_Project.Service
{
    public class PharmacyService : IPharmacyService
    {
        public object GetNearestPharmacies(double Patient_Latitude, double Patient_Longitude, IReadOnlyList<Pharmacy> pharmacies)
        {
            return pharmacies
                    .Select(p => new PharmacyWithDistances
                    {
                        pharmacy = p,
                        Distance = CalculateDistance(Patient_Latitude, Patient_Longitude, p.Latitude, p.Longitude)
                    })
                    .OrderBy(p => p.Distance)
                    .Take(6)
                    .ToList();
                    
        }
        public object GetDefaultNearestPharmacies(double Patient_Longitude, double Patient_Latitude, IReadOnlyList<Pharmacy> pharmacies)
        {
            return pharmacies
                    .Select(ph => new PharmacyWithDistances
                    {
                        pharmacy = ph,
                        Distance = CalculateDistance(Patient_Latitude, Patient_Longitude, ph.Latitude, ph.Longitude)
                    })
                    .OrderBy(d => d.Distance)
                    .Take(10)
                    .ToList();
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in kilometers
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }
    }
}

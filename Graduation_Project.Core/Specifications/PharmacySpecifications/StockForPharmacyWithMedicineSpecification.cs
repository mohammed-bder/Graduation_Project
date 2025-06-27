using Graduation_Project.Core.Models.Pharmacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Graduation_Project.Core.Specifications.PharmacySpecifications
{
    public class StockForPharmacyWithMedicineSpecification : BaseSpecifications<PharmacyMedicineStock>
    {
        public StockForPharmacyWithMedicineSpecification(StockSpecParams specParams) 
            : base(pm => 
                (pm.PharmacyId == specParams.pharmacyId)
                &&
                (
                    string.IsNullOrEmpty(specParams.Search) ||
                    pm.Medicine.Name_en.ToLower().StartsWith(specParams.Search)
                )
            )
        {
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "name_asc":
                        AddOrderBy(s => s.Medicine.Name_en);
                        break;
                    case "name_desc":
                        AddOrderByDescending(s => s.Medicine.Name_en);
                        break;
                    case "quantity_asc":
                        AddOrderBy(s => s.Quantity);
                        break;
                    case "quantity_desc":
                        AddOrderByDescending(s => s.Quantity);
                        break;
                    case "price_asc":
                        AddOrderBy(s => s.Medicine.Price);
                        break;
                    case "price_desc":
                        AddOrderByDescending(s => s.Medicine.Price);
                        break;
                    default:
                        // Default sorting (perhaps by name ascending)
                        AddOrderBy(s => s.Medicine.Name_en);
                        break;
                }
            }

            Includes.Add(m => m.Medicine);
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }
    }
}

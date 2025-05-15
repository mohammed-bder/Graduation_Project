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

                (string.IsNullOrEmpty(specParams.Search) ||
                    (
                        !string.IsNullOrEmpty(specParams.FirstNameSearch) &&
                        (
                            (string.IsNullOrEmpty(specParams.LastNameSearch) &&
                                (d.FirstName.ToLower().Contains(specParams.FirstNameSearch) || d.LastName.ToLower().Contains(specParams.FirstNameSearch))
                            )
                            ||
                            (!string.IsNullOrEmpty(specParams.LastNameSearch) &&
                                d.FirstName.ToLower().Contains(specParams.FirstNameSearch) &&
                                d.LastName.ToLower().Contains(specParams.LastNameSearch)
                            )
                        )
                    )
                )
                &&
                //(!specParams.MaxPrice.HasValue || d.ConsultationFees <= specParams.MaxPrice) &&
                (
                    (!specParams.MaxPrice.HasValue || specParams.MaxPrice > 1000 || d.ConsultationFees <= specParams.MaxPrice)
                    &&
                    (!specParams.MinPrice.HasValue || d.ConsultationFees >= specParams.MinPrice)
                ) &&

                (!specParams.Gender.HasValue || d.Gender == specParams.Gender) &&
                (!specParams.SubSpecialtyId.HasValue || d.DoctorSubspeciality.Any(ds => ds.SubSpecialitiesId == specParams.SubSpecialtyId.Value)) &&
                (!specParams.SpecialtyId.HasValue || d.SpecialtyId == specParams.SpecialtyId) &&
                // ✅ Apply Availability Filter
                (
                    (!specParams.Availability.HasValue) ||
                    (specParams.Availability == AvailabilityFilter.AllTimes &&
                        (d.ScheduleExceptions.Any(se => se.IsAvailable) || d.WorkSchedules.Any())
                    ) ||                                    // ✅ Ensure doctor has at least one schedule
                    (specParams.Availability == AvailabilityFilter.Today &&
                        (
                            d.ScheduleExceptions.Any(se => se.Date == DateOnly.FromDateTime(specParams.Today) && se.IsAvailable)
                            ||
                            (!d.ScheduleExceptions.Any(se => se.Date == DateOnly.FromDateTime(specParams.Today) && !se.IsAvailable)
                             && d.WorkSchedules.Select(ws => ws.Day).Contains(specParams.Today.DayOfWeek))
                        )
                    ) ||
                    (specParams.Availability == AvailabilityFilter.Tomorrow &&
                        (
                            d.ScheduleExceptions.Any(se => se.Date == DateOnly.FromDateTime(specParams.Tomorrow) && se.IsAvailable)
                            ||
                            (!d.ScheduleExceptions.Any(se => se.Date == DateOnly.FromDateTime(specParams.Tomorrow) && !se.IsAvailable)
                             && d.WorkSchedules.Select(ws => ws.Day).Contains(specParams.Tomorrow.DayOfWeek))
                        )
                    )
                )
            //)
            )
        {
            
        }

    }
}

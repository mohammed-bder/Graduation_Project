using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graduation_Project.Repository.Data
{
    public static class ApplicationDbContextSeed
    {
        // 1. read file
        // 2. deserialize json file into list of Models
        // 3. iterate this list and add this into dbContext

        public async static Task SeedAsync(ApplicationDbContext _dbContext)
        {
            /* ======================= seeding Doctor Data ======================= */
            //if(_dbContext.Doctors.Count() == 0)
            //{
            //    var doctorData = File.ReadAllText("");
            //    var doctors = JsonSerializer.Deserialize<List<Doctor>>(doctorData);

            //    if(doctors?.Count() >= 0)
            //    {
            //        foreach (var doctor in doctors)
            //        {
            //            _dbContext.Doctors.Add(doctor);
            //        }
            //        await _dbContext.SaveChangesAsync();
            //    }
            //}

            // repeat this for each entity

            /* ======================= seeding Speciality & SubSpecialities Data ======================= */
            if (_dbContext.Specialties.Count() == 0)
            {
                var SpecialtyData = File.ReadAllText("../Graduation_Project.Repository/Data/DataSeed/Specialty.json");
                var specialties = JsonSerializer.Deserialize<List<Specialty>>(SpecialtyData);

                if (specialties?.Count > 0)
                {
                    foreach (var specialty in specialties)
                    {
                        if (specialty.SubSpecialities != null && specialty.SubSpecialities.Count > 0)
                        {
                            foreach (var subSpecialty in specialty.SubSpecialities)
                            {
                                // Set the Specialty reference explicitly
                                subSpecialty.Specialty = specialty;
                            }
                        }

                        _dbContext.Specialties.Add(specialty);
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }


        }
    }
}

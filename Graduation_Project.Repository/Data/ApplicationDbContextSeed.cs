using Graduation_Project.Core.Models.Clinics;
using Graduation_Project.Core.Models.Pharmacies;
using System;
using System.Collections.Generic;
using System.Data;
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

            /* ======================= seeding Governrate Data ======================= */
            if (!await _dbContext.governorates.AnyAsync())
            {
                var GovernorateData = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/Governorates.json");
                var governorates = JsonSerializer.Deserialize<List<Governorate>>(GovernorateData);

                if (governorates?.Count > 0)
                {
                    Console.WriteLine($"Total governorates in JSON: {governorates.Count}");

                    await _dbContext.governorates.AddRangeAsync(governorates);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Seeding completed. Inserted: {await _dbContext.governorates.CountAsync()} records.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting records: {ex.Message}");
                    }
                }
            }

            /* ======================= seeding Regions Data ======================= */
            if (!await _dbContext.regions.AnyAsync())
            {
                var RegionsData = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/Regions.json");
                var Regions = JsonSerializer.Deserialize<List<Region>>(RegionsData);

                if (Regions?.Count > 0)
                {
                    Console.WriteLine($"Total records in JSON: {Regions.Count}");

                    await _dbContext.regions.AddRangeAsync(Regions);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Seeding completed. Inserted: {await _dbContext.regions.CountAsync()} records.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting records: {ex.Message}");
                    }
                }
            }

            /* ======================= seeding Medical Category Data ======================= */
            if(!await _dbContext.MedicalCategories.AnyAsync())
            {
                var MedicalCategoryData = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/MedicalCategory.json");
                var MedicalCategories = JsonSerializer.Deserialize<List<MedicalCategory>>(MedicalCategoryData);

                if (MedicalCategories?.Count > 0)
                {
                    Console.WriteLine($"Total records in JSON: {MedicalCategories.Count}");
                    await _dbContext.MedicalCategories.AddRangeAsync(MedicalCategories);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Seeding completed. Inserted: {await _dbContext.MedicalCategories.CountAsync()} records.");

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error inserting records: {ex.Message}");
                    }
                }
            }

            /* ======================= seeding Medicine Data ======================= */
            if(! await _dbContext.Medicines.AnyAsync())
            {
                var MedicineData = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/Medicines.json");
                var medicines = JsonSerializer.Deserialize<List<Medicine>>(MedicineData);

                if (medicines?.Count > 0)
                {
                    Console.WriteLine($"Total records in JSON: {medicines.Count}");
                    await _dbContext.Medicines.AddRangeAsync(medicines);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Seeding completed. Inserted: {await _dbContext.Medicines.CountAsync()} records.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting records: {ex.Message}");
                    }
                }
            }

            /* ======================= seeding pharmacy Data ======================= */

            if(! await _dbContext.Pharmacies.AnyAsync())
            {
                var pharmacyData = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/pharmacy/pharmacy.json");
                var pharmacy = JsonSerializer.Deserialize<Pharmacy>(pharmacyData);

                if(pharmacy is not null)
                {
                    await _dbContext.AddAsync(pharmacy);
                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Seeding completed. Inserted: {await _dbContext.Medicines.CountAsync()} records.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting records: {ex.Message}");
                    }
                }

            }


            if (!await _dbContext.PharmacyContacts.AnyAsync())
            {
                var data = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/pharmacy/pharmacy_contacts.json");
                var contacts = JsonSerializer.Deserialize<List<PharmacyContact>>(data);

                if (contacts?.Count > 0)
                {
                    Console.WriteLine($"Contacts in JSON: {contacts.Count}");
                    await _dbContext.PharmacyContacts.AddRangeAsync(contacts);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Contacts seeding done. Inserted: {await _dbContext.PharmacyContacts.CountAsync()}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting Contacts: {ex.Message}");
                    }
                }
            }
            if (!await _dbContext.PharmacyOrders.AnyAsync())
            {
                var data = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/pharmacy/pharmacy_orders.json");
                var orders = JsonSerializer.Deserialize<List<PharmacyOrder>>(data);

                if (orders?.Count > 0)
                {
                    Console.WriteLine($"Orders in JSON: {orders.Count}");
                    await _dbContext.PharmacyOrders.AddRangeAsync(orders);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Orders seeding done. Inserted: {await _dbContext.PharmacyOrders.CountAsync()}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting Orders: {ex.Message}");
                    }
                }
            }

            if (!await _dbContext.PharmacyMedicineStocks.AnyAsync())
            {
                var data = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/pharmacy/pharmacy_medicine_stocks.json");
                var stocks = JsonSerializer.Deserialize<List<PharmacyMedicineStock>>(data);

                if (stocks?.Count > 0)
                {
                    Console.WriteLine($"Stocks in JSON: {stocks.Count}");
                    await _dbContext.PharmacyMedicineStocks.AddRangeAsync(stocks);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Stocks seeding done. Inserted: {await _dbContext.PharmacyMedicineStocks.CountAsync()}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting Stocks: {ex.Message}");
                    }
                }
            }

            if (!await _dbContext.MedicinePharmacyOrders.AnyAsync())
            {
                var data = await File.ReadAllTextAsync("../Graduation_Project.Repository/Data/DataSeed/pharmacy/medicine_pharmacy_orders.json");
                var medicinePharmacyOrders = JsonSerializer.Deserialize<List<MedicinePharmacyOrder>>(data);

                if (medicinePharmacyOrders?.Count > 0)
                {
                    Console.WriteLine($"Stocks in JSON: {medicinePharmacyOrders.Count}");
                    await _dbContext.MedicinePharmacyOrders.AddRangeAsync(medicinePharmacyOrders);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        Console.WriteLine($"Stocks seeding done. Inserted: {await _dbContext.PharmacyMedicineStocks.CountAsync()}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inserting Stocks: {ex.Message}");
                    }
                }
            }




        }
    }
}

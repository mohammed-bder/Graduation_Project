using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models.Pharmacies;
using Pharmacy_Dashboard.MVC.ViewModel.Dashboard;

namespace Pharmacy_Dashboard.MVC.Repository
{
    public interface IOrderRepository : IGenericRepository<PharmacyOrder>
    {

        public Task<List<TopMedicineViewModel>> GetTop5MedicinesAsync(int pharmacyId);
    }
}

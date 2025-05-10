using AutoMapper;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Core.Models.Shared;
using Pharmacy_Dashboard.MVC.ViewModel.Account;
using Pharmacy_Dashboard.MVC.ViewModel.Stock;

namespace Pharmacy_Dashboard.MVC.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            /****************************************** Mapping for Pharmacy Register ******************************************/
            CreateMap<RegisterViewModel , Pharmacy>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PharmacyName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.LicenseImageUrl, opt => opt.MapFrom(src => src.LicenseImageUrl));

            /****************************************** Mapping for Stock ******************************************/
            CreateMap<PharmacyMedicineStock, PharmacyStockViewModel>()
                .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine.Name_en))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Medicine.Price));
        }
    }
}

using AutoMapper;
using Graduation_Project.Core.Models.Pharmacies;
using Pharmacy_Dashboard.MVC.ViewModels.OrderViewModels;

namespace Pharmacy_Dashboard.MVC.helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            //CreateMap<>

            // ========================================== Pharmacy Order ==========================================
            CreateMap<PharmacyOrder, OrderContent>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName));

            CreateMap<MedicinePharmacyOrder, OrderMedicine>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MedicineId))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Medicine.Price))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Medicine.Name_en));

            CreateMap<PharmacyOrder, OrderCardViewModel>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.PatientPhone, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.OrderMedicines, opt => opt.MapFrom(src => src.MedicinePharmacyOrders));


        }
    }
}

using AutoMapper;
using Graduation_Project.Core.Models.Pharmacies;
using Pharmacy_Dashboard.MVC.ViewModel.Account;

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

            /****************************************** Mapping for Pharmacy Edit info ******************************************/
            CreateMap<Pharmacy, EditProfileViewModel>()
                .ForMember(dest => dest.PharmacyName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<Pharmacy , EditProfileViewModel>>())
                .ForMember(dest => dest.PharmacyContacts, opt => opt.MapFrom(src => src.pharmacyContacts.Select(c => new PharmacyContactViewModel
                {
                    Id = c.Id,
                    PhoneNumber = c.PhoneNumber,
                })));


            CreateMap<EditProfileViewModel, Pharmacy>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PharmacyName))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.PictureUrl))
                .ForMember(dest => dest.pharmacyContacts, opt => opt.Ignore());




        }
    }
}

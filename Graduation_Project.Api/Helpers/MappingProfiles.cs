using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.Helpers;
namespace Graduation_Project.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Person, PersonToReturnDTO>()
                .ForMember(d => d.PictureUrl, O => O.MapFrom<PersonPictureUrlResolver>());

            CreateMap<Specialty, SpecialityDTO>();
            CreateMap<SubSpecialities, SubSpecialityDTO>()
                .ForMember(s => s.Specialty, O => O.MapFrom(s => s.Specialty.Name));
        }
    }
}

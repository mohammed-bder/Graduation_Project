using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Doctor;
using Graduation_Project.Api.DTO.Patient;
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

            CreateMap<Doctor, DoctorForProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ));

            CreateMap<DoctorForProfileDto, Doctor>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src =>
                    src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]
                ))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src =>
                    src.FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length > 2
                    ? string.Join(" ", src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1))
                    : src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]
                ));

            CreateMap<Patient, PatientForProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ));

            CreateMap<PatientForProfileDto, Patient>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src =>
                    src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]
                ))

                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src =>
                    src.FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length > 2
                    ? string.Join(" ", src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1))
                    : src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]
                ));

            CreateMap<Doctor, SortingDoctorDto>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ));

        }
    }
}

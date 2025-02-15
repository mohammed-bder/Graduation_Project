using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Doctor;
using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.Helpers;
using System.Globalization;
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
                ))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
                    src.Gender.ToString()
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
                ))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
                    src.Gender.ToString()
                ))
                .ForMember(dest => dest.BloodTrpe, opt => opt.MapFrom(src =>
                    src.BloodType.ToString()
                ));

            CreateMap<PatientForProfileDto, Patient>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src =>
                    src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]
                ))

                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src =>
                    src.FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length > 2
                    ? string.Join(" ", src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1))
                    : src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]
                ))

                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src =>
                     src.BloodTrpe == null ? null : Enum.Parse(typeof(BloodType), src.BloodTrpe)
                     ));

            CreateMap<Doctor, SortingDoctorDto>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
                    src.Gender.ToString()
                ));

            /****************************************** Mapping for Medicl Category ******************************************/
            CreateMap<MedicalCategory , MedicalCategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.Name
                ));


            /****************************************** Mapping for Medicl History ******************************************/
            CreateMap<MedicalHistory, MedicalHistoryDto>()
                .ForMember(dest => dest.MedicalCategory, opt => opt.MapFrom(src => src.MedicalCategory.Name))
                .ForMember(dest => dest.MedicalImage, opt => opt.MapFrom<MedicalHistoryPictureUrlResolver>());

            CreateMap<MedicalHistoryFormDto, MedicalHistory>();

            CreateMap<MedicalHistory, MedicalHistoryFormDto>();

            CreateMap<MedicalHistory, MedicalHistoryInfoDto>();
                

        }
    }
}

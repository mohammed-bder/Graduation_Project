using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Clinics;
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

            CreateMap<Doctor, DoctorDetailsDto>()
               .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src =>
                    src.Specialty != null ? src.Specialty.Name : null
                ))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
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
                

            CreateMap<Education, EducationDto>();

            CreateMap<EducationDto, Education>();

            CreateMap<Clinic, DoctorAboutClinicDto>();

            CreateMap<Education, DoctorAboutDto>();



            // ========================================== Clinic ==========================================
            CreateMap<Clinic, ClinicDTO>()
                .ForMember(dest => dest.RegionName, O => O.MapFrom(src => src.Region.Name))
                .ForMember(dest => dest.GovernorateName, O => O.MapFrom(src => src.Region.governorate.Name))
                .ForMember(dest => dest.GovernorateId, O => O.MapFrom(src => src.Region.governorate.Id))
                ;
        }
    }
}

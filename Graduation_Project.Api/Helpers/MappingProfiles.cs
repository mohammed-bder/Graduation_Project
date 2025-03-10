using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Api.DTO.FeedBacks;
using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.DTO.Shared;
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

            //CreateMap<Specialty, SpecialityDTO>();
            //CreateMap<SubSpecialities, SubSpecialityDTO>()
            //    .ForMember(s => s.Specialty, O => O.MapFrom(s => s.Specialty.Name_ar));

            /****************************************** Mapping for Doctor Profile ******************************************/

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

            /****************************************** Mapping for Patient Profile ******************************************/

            CreateMap<Patient, PatientForProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + " " + src.LastName
                ));
                //.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                //    DateOnly.FromDateTime(src.DateOfBirth.Value.Date)
                //));


            CreateMap<PatientForProfileDto, Patient>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src =>
                    src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]
                ))

                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src =>
                    src.FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length > 2
                    ? string.Join(" ", src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1))
                    : src.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]
                ));

            /****************************************** Mapping for Home ******************************************/

            CreateMap<Doctor, SortingDoctorDto>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ));

            /****************************************** Mapping for Doctor From Patient ******************************************/

            CreateMap<Doctor, DoctorDetailsDto>()
               .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src =>
                    src.Specialty != null ? src.Specialty.Name_ar : null
                ))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ));

            /****************************************** Mapping for Education& Clinic ******************************************/
            CreateMap<Education, EducationDto>();

            CreateMap<EducationDto, Education>();

            CreateMap<ClinicAboutDto, DoctorAboutDto>();


            CreateMap<Education, DoctorAboutDto>();

            /****************************************** Mapping for Medicl History ******************************************/
            CreateMap<MedicalHistory, MedicalHistoryDto>()
                .ForMember(dest => dest.MedicalCategory, opt => opt.MapFrom(src => src.MedicalCategory.Name_ar))
                .ForMember(dest => dest.MedicalImage, opt => opt.MapFrom<MedicalHistoryPictureUrlResolver>());

            CreateMap<MedicalHistoryFormDto, MedicalHistory>();

            CreateMap<MedicalHistory, MedicalHistoryFormDto>();

            CreateMap<MedicalHistory, MedicalHistoryInfoDto>();

            /****************************************** Mapping for Feedback ******************************************/
            CreateMap<FeedbackDto, Feedback>();
            CreateMap<Feedback, FeedbackInfoDto>();
            CreateMap<Feedback, FeedbackWithIdToReturnDto>();



            CreateMap<Feedback, FeedbackToReturnDto>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                        src.patient.FirstName + ' ' + src.patient.LastName
                    ))
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                        src.patient.DateOfBirth.HasValue
                            ? (DateTime.Today.Year - src.patient.DateOfBirth.Value.Year) -
                              (DateTime.Today.DayOfYear < src.patient.DateOfBirth.Value.DayOfYear ? 1 : 0)
                            : (int?)null
                    ));


            // ========================================== Clinic ==========================================
            CreateMap<Clinic, ClinicInfoDTO>()
                .ForMember(dest => dest.RegionName, O => O.MapFrom(src => src.Region.Name_en))
                .ForMember(dest => dest.GovernorateName, O => O.MapFrom(src => src.Region.governorate.Name_en))
                .ForMember(dest => dest.GovernorateId, O => O.MapFrom(src => src.Region.governorate.Id))
                ;



            // ========================================== Governorate ==========================================

            CreateMap<Governorate, GovernorateDTO>();


            // ========================================== Region ==========================================

            CreateMap<Region, RegionDTO>();

            // ========================================== Prescription ==========================================
            CreateMap<PrescriptionFromUserDto, Prescription>()
                .ForMember(dest => dest.MedicinePrescriptions, opt => opt.MapFrom(src => src.MedicinePrescriptions))
                .ReverseMap();

            CreateMap<Prescription, PrescriptionEditFormDto>()
                .ForMember(dest => dest.MedicinePrescriptions, opt => opt.MapFrom(src => src.MedicinePrescriptions));

            // ========================================== Medicine ==========================================
            CreateMap<MedicinePrescriptionDto, MedicinePrescription>()
            .ForMember(dest => dest.PrescriptionId, opt => opt.Ignore()) // Ignore PrescriptionId
            .ForMember(dest => dest.Prescription, opt => opt.Ignore())   // Ignore Prescription
            .ForMember(dest => dest.Medicine, opt => opt.Ignore())       // Ignore Medicine
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ReverseMap();

            // ========================================== WorkSchedule ==========================================
            CreateMap<WorkScheduleFromUserDto, WorkSchedule>();
            CreateMap<WorkSchedule, WorkScheduleFromDatabaseDto>();


        }
    }
}

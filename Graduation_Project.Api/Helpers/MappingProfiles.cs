using AutoMapper;
using Graduation_Project.Api.DTO;
using Graduation_Project.Api.DTO.Clinics;
using Graduation_Project.Api.DTO.Doctors;
using Graduation_Project.Api.DTO.FeedBacks;
using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.DTO.Shared;
using Graduation_Project.Api.Helpers;
using Microsoft.Extensions.Configuration;
using System.Globalization;
namespace Graduation_Project.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Person, PersonToReturnDTO>()
                .ForMember(d => d.PictureUrl, O => O.MapFrom<PictureUrlResolver<Person, PersonToReturnDTO>>()); 

            //CreateMap<Specialty, SpecialityDTO>();
            //CreateMap<SubSpecialities, SubSpecialityDTO>()
            //    .ForMember(s => s.Specialty, O => O.MapFrom(s => s.Specialty.Name_ar));

            /****************************************** Mapping for Doctor Profile ******************************************/

            CreateMap<Doctor, DoctorForProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ))
                .ForMember(dest => dest.PictureUrl,opt => opt.MapFrom<PictureUrlResolver<Doctor, DoctorForProfileDto>>());

            CreateMap<Doctor, DoctorForProfileToReturnDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                )).ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<Doctor, DoctorForProfileToReturnDto>>());

            

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
                ))
                .ForMember(dest => dest.PictureUrl , opt => opt.MapFrom<PictureUrlResolver<Patient, PatientForProfileDto>>());
                //.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                //    DateOnly.FromDateTime(src.DateOfBirth.Value.Date)
                //));

            CreateMap<Patient, PatientForProfileToReturnDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + " " + src.LastName
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

            /****************************************** Mapping for Home ******************************************/
            CreateMap<Doctor, SortingDoctorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src =>
                    src.Specialty != null ? src.Specialty.Name_ar : null
                ))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<Doctor, SortingDoctorDto>>());

            /****************************************** Mapping for Doctor From Patient ******************************************/

            CreateMap<Doctor, DoctorDetailsDto>()
               .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src =>
                    src.Specialty != null ? src.Specialty.Name_ar : null
                ))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.FirstName + ' ' + src.LastName
                ))
               .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<Doctor, DoctorDetailsDto>>());

            /****************************************** Mapping for Education& Clinic ******************************************/
            CreateMap<Education, EducationDto>();

            CreateMap<EducationDto, Education>();

            CreateMap<ClinicAboutDto, DoctorAboutDto>();


            CreateMap<Education, DoctorAboutDto>();

            /****************************************** Mapping for Medicl History ******************************************/
            CreateMap<MedicalHistory, MedicalHistoryDto>()
                .ForMember(dest => dest.MedicalCategory, opt => opt.MapFrom(src => src.MedicalCategory.Name_ar))
                .ForMember(dest => dest.MedicalImage,  O => O.MapFrom<PictureUrlResolver<MedicalHistory, MedicalHistoryDto>>()); 

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
            CreateMap<Prescription, PrescriptionEditFormDto>()
                .ForMember(dest => dest.MedicinePrescriptions, opt => opt.MapFrom(src => src.MedicinePrescriptions));


            CreateMap<PrescriptionImageDTO, PrescriptionImage>();


            // ========================================== Medicine ==========================================
            CreateMap<MedicinePrescriptionDto, MedicinePrescription>()
            .ForMember(dest => dest.PrescriptionId, opt => opt.Ignore()) // Ignore PrescriptionId
            .ForMember(dest => dest.Prescription, opt => opt.Ignore())   // Ignore Prescription
            .ForMember(dest => dest.Medicine, opt => opt.Ignore())       // Ignore Medicine
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ReverseMap();


            CreateMap<Prescription, PrescriptionResponseDTO>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.PatientAge, opt => opt.MapFrom(src => src.Patient.DateOfBirth.HasValue ? (int?)(DateTime.Now.Year - src.Patient.DateOfBirth.Value.Year) : null))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FirstName + " " + src.Doctor.LastName))
                .ForMember(dest => dest.IssuedDate , opt => opt.MapFrom(src => src.IssuedDate.ToString("yyyy-MM-dd HH:mm:ss")))
                
                ;

            CreateMap<PrescriptionImage, PrescriptionImageDTO>();

            CreateMap<MedicinePrescription, MedicinePrescriptionResponseDTO>()
             .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine.Name_en))
             .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));

            // ========================================== WorkSchedule ==========================================
            CreateMap<WorkScheduleFromUserDto, WorkSchedule>();
            CreateMap<WorkSchedule, WorkScheduleFromDatabaseDto>();
            // ========================================== ScheduleException ==========================================
            CreateMap<ScheduleExceptionFromUserDto, ScheduleException>();
            CreateMap<ScheduleException, ScheduleExceptionFromDatabaseDto>();

            // ========================================== Appointment ==========================================
            CreateMap<BookAppointmentDto, Appointment>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.AppointmentTime.ToString("HH:mm:ss")))
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())); // Convert enum to string

            CreateMap<Appointment, AppointmentForPatientDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
                .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.AppointmentTime.ToString("HH:mm:ss")))
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())); // Convert enum to string

        }
    }
}

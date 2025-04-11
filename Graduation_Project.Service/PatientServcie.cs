using Graduation_Project.Core;
using Graduation_Project.Core.Enums;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Models.Patients;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Service
{
    public class PatientServcie : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PatientServcie(IUnitOfWork unitOfWork , IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<object?> GetInfo(int Id, string Email)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetAsync(Id);
            if (patient is null)
                return null;

            if (!string.IsNullOrEmpty(patient.PictureUrl))
                patient.PictureUrl = patient.PictureUrl[0] == '/' ? $"{_configuration["ServerUrl"]}{patient.PictureUrl}" : $"{_configuration["ServerUrl"]}/{patient.PictureUrl}";

            if(Email is null)
            {
                
            }
            
            var patientInfo = new
            {
                fullName = patient.FirstName + " " + patient.LastName,
                email = Email,
                phoneNumber = patient.PhoneNumber,
                gender = patient.Gender,
                dateOfBirth = patient.DateOfBirth,
                pictureUrl = patient.PictureUrl,
                bloodType = patient.BloodType
            };
            return patientInfo;
        }
    }
}
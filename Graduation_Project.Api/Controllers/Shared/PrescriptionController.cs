using AutoMapper;
using Graduation_Project.Api.DTO.Pharmacies;
using Graduation_Project.Api.DTO.Shared;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Shared;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.MedicineSpecifications;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Graduation_Project.Core.Specifications.PrescriptionSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Talabat.API.Dtos.Account;

namespace Graduation_Project.Api.Controllers.Shared
{
    public class PrescriptionController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PrescriptionController(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPost("DoctorAdd")]
        public async Task<ActionResult<PrescriptionFromUserDto>> AddPrescription(PrescriptionFromUserDto prescriptionFromUser)
        {
            var DoctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));

            var prescription = _mapper.Map<PrescriptionFromUserDto, Prescription>(prescriptionFromUser);
            prescription.IssuedDate = DateTime.Now;
            prescription.DoctorId = DoctorId;

            try
            {
                var createdPrescription = await _unitOfWork.Repository<Prescription>().AddWithSaveAsync(prescription);

                if (!prescriptionFromUser.MedicinePrescriptions.IsNullOrEmpty())
                {
                    var medicinePrescriptions = prescriptionFromUser.MedicinePrescriptions.Select(medicinePrescription => new MedicinePrescription
                    {
                        PrescriptionId = createdPrescription.Id,
                        MedicineId = medicinePrescription.MedicineId,
                        Details = medicinePrescription.Details
                    }).ToList();

                    await _unitOfWork.Repository<MedicinePrescription>().AddRangeAsync(medicinePrescriptions);
                }
                
                return Ok(prescriptionFromUser);
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }

        }

        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpPut("DoctorEdit/{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Prescription>))]
        public async Task<ActionResult<PrescriptionEditFormDto>> EditPrescription(PrescriptionEditFormDto updateDto, int id)
        {
            var spec = new PrescriptionWithMedicinePrescriptionsSpec(id);
            var prescriptionFromDB = await _unitOfWork.Repository<Prescription>().GetWithSpecsAsync(spec);
            if(prescriptionFromDB is null)
            {
                return (NotFound(new ApiResponse(404)));
            }
            //check if the doctor who wrote it is the same person who is editing
            var DoctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            if(prescriptionFromDB.Id != DoctorId)
            {
                return (Unauthorized(new ApiResponse(401, "This Doctor is not Authorized to Edit this")));
            }

            if ((DateTime.UtcNow - prescriptionFromDB.IssuedDate).TotalHours > 24)
            {
                throw new InvalidOperationException("❌ You cannot edit this prescription after 24 hour of creation.");
            }

            //prescriptionFromUser.PatientId = prescriptionFromDB.Id;
            prescriptionFromDB.Diagnoses = updateDto.Diagnoses;
            
            var existingMedicinePrescriptions = prescriptionFromDB.MedicinePrescriptions?.ToList();

            if (existingMedicinePrescriptions.Any())
            {
                var newMedicinePrescriptions = updateDto.MedicinePrescriptions
                    .Where(mp => !existingMedicinePrescriptions.Any(emp => emp.MedicineId == mp.MedicineId))
                    .Select(mp => new MedicinePrescription
                    {
                        PrescriptionId = id,
                        MedicineId = mp.MedicineId,
                        Details = mp.Details
                    }).ToList();
                var medicineToBeRemoved = existingMedicinePrescriptions
                    .Where(emp => !updateDto.MedicinePrescriptions.Any(mp => mp.MedicineId == emp.MedicineId))
                    .ToList();

                if (medicineToBeRemoved.Any())
                {
                    _unitOfWork.Repository<MedicinePrescription>().DeleteRange(medicineToBeRemoved);
                }

                if (newMedicinePrescriptions.Any())
                {
                    await _unitOfWork.Repository<MedicinePrescription>().AddRangeAsync(newMedicinePrescriptions);
                }
            }

            try
            {
                _unitOfWork.Repository<Prescription>().Update(prescriptionFromDB);
                
                var hasChanges = _unitOfWork.HasChanges();
                var result = await _unitOfWork.CompleteAsync();
                
                if (hasChanges && result == 0)
                {
                    return BadRequest(new ApiResponse(400));
                }

                return Ok(_mapper.Map<Prescription, PrescriptionEditFormDto>(prescriptionFromDB));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }

        }


        [Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpDelete("DoctorDelete/{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Prescription>))]
        public async Task<ActionResult> DeletePrescription(int id)
        {
            var spec = new PrescriptionWithMedicinePrescriptionsSpec(id);
            var prescriptionFromDB = await _unitOfWork.Repository<Prescription>().GetWithSpecsAsync(spec);
            if(prescriptionFromDB is null)
            {
                return NotFound(new ApiResponse(404));
            }

            var DoctorId = int.Parse(User.FindFirstValue(Identifiers.DoctorId));
            if (prescriptionFromDB.Id != DoctorId)
            {
                return (Unauthorized(new ApiResponse(401, "This Doctor is not Authorized to Delete this")));
            }

            if ((DateTime.UtcNow - prescriptionFromDB.IssuedDate).TotalHours > 24)
            {
                throw new InvalidOperationException("❌ You cannot Delete this prescription after 24 hour of creation.");
            }

            if (prescriptionFromDB.MedicinePrescriptions.Any())
            {
                _unitOfWork.Repository<MedicinePrescription>().DeleteRange(prescriptionFromDB.MedicinePrescriptions);
            }
            _unitOfWork.Repository<Prescription>().Delete(prescriptionFromDB);

            var result = await _unitOfWork.CompleteAsync();
            if(result == 0)
            {
                return BadRequest(new ApiResponse(400));
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Prescription deleted Successfully"));
        }


        [Authorize(Roles = $"{nameof(UserRoleType.Doctor)},{nameof(UserRoleType.Patient)}")]
        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Prescription>> GetPrescriptionById(int id)
        {
            var spec = new PrescriptionWithMedicinePrescriptionsSpec(id);
            var prescriptionFromDB = await _unitOfWork.Repository<Prescription>().GetWithSpecsAsync(spec);
            if(prescriptionFromDB is null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<Prescription, PrescriptionEditFormDto>(prescriptionFromDB));

        }

        [Authorize(Roles = $"{nameof(UserRoleType.Doctor)},{nameof(UserRoleType.Patient)}")]
        [HttpGet("GetAllForPatient")]
        public async Task<ActionResult<IReadOnlyList<Prescription>>> GetAllPrescription(int? id)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var userRole = await _userService.GetUserRoleAsync(currentUser);

            //Check if the current user is a patient
            if (userRole == nameof(UserRoleType.Patient))
            {
                var patientId = int.Parse(User.FindFirstValue(Identifiers.PatientId));

                var pspec = new PatientForProfileSpecs(patientId);
                var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(pspec);
                if (patient is null)
                {
                    return NotFound(new ApiResponse(404));
                }
                id = patient.Id; // Override 'id' if patient
            }
            else if (userRole == nameof(UserRoleType.Doctor))
            {
                // If the request comes from a doctor, ensure the 'id' parameter is provided.
                if (!id.HasValue)
                {
                    return BadRequest("Patient ID is required when a doctor is requesting.");
                }
            }
            
            var spec = new AllPrescriptionsForPatientWithMedicinePrescriptionsSpec(id.Value);
            var prescriptionsFromDB = await _unitOfWork.Repository<Prescription>().GetAllWithSpecAsync(spec);
            if (prescriptionsFromDB.IsNullOrEmpty())
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<IReadOnlyList<Prescription>, IReadOnlyList<PrescriptionEditFormDto>>(prescriptionsFromDB));
        }



    }
}

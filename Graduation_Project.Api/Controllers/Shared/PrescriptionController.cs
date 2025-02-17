using AutoMapper;
using Graduation_Project.Api.DTO.Shared;
using Graduation_Project.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos.Account;

namespace Graduation_Project.Api.Controllers.Shared
{
    public class PrescriptionController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrescriptionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //[HttpPost("DoctorAdd")]
        //public async Task<ActionResult<IReadOnlyList<Prescription>>> AddPrescription(PrescriptionFromUserDto userDto)
        //{
        //    var prescription = new Prescription
        //    {
        //        DoctorId = userDto.DoctorId,
        //        PatientId = userDto.PatientId,
        //        Diagnoses = userDto.Diagnoses,
        //        IssuedDate = DateTime.Now,
        //        MedicinePrescriptions = userDto.MedicinePrescriptions.Select(m => new MedicinePrescription
        //        {
        //            PrescriptionId = m.PrescriptionId,
        //            MedicineId = m.MedicineId,
        //            Details = m.Details
        //        }).ToHashSet()  // Using HashSet for ICollection
        //    };

        //    return true; 
        //}
    }
}

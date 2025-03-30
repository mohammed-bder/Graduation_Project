using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO.FeedBacks;
using Graduation_Project.Api.DTO.Patients;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Shared;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Graduation_Project.Core.Specifications.FeedBackSpecifications;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Graduation_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.PatientControllers
{
    public class FeedbackController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public FeedbackController(IUnitOfWork unitOfWork , IUserService userService ,INotificationService notificationService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        /****************************************** Add Feedback ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPost("add-feedback")]
        public async Task<ActionResult<FeedbackInfoDto>> AddFeedback(FeedbackDto feedbackDto)
        {
            var patientId = int.Parse(User.FindFirstValue(Identifiers.PatientId));
            var spec = new PatientForProfileSpecs(patientId);

            var patient = await _unitOfWork.Repository<Patient>().GetWithSpecsAsync(spec);

            if(patient is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Patient Not Found"));
            }

            var feedBack = _mapper.Map<FeedbackDto, Feedback>(feedbackDto);
            feedBack.PatientId = patient.Id;
            feedBack.Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Egypt Standard Time");

            try
            {
                await _unitOfWork.Repository<Feedback>().AddAsync(feedBack);
                var result = await _unitOfWork.CompleteAsync();
                if(result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add Feedback"));
                }
                await UpdateDoctorRating(feedBack.DoctorId);
                // push notification 
                var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(feedbackDto.DoctorId);
                await _notificationService.SendNotificationAsync(doctor.ApplicationUserId, "A new patient write a feedback for you..", "New FeedBack!!");

                return Ok(_mapper.Map<Feedback,FeedbackInfoDto>(feedBack));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }

        /****************************************** Edit Feedback ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPut("edit-feedback/{Id:int}")]
        public async Task<ActionResult<FeedbackInfoDto>> EditFeedback(FeedbackDto feedbackDto , int Id)
        {
            var feedback = await _unitOfWork.Repository<Feedback>().GetAsync(Id);
            if(feedback is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Feedback Not Found"));
            }

            var feedbackMapped = _mapper.Map(feedbackDto, feedback);
            feedbackMapped.Date = DateTime.Now;
            try
            {
                _unitOfWork.Repository<Feedback>().Update(feedback);
                var result = await _unitOfWork.CompleteAsync();
                if(result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to update Feedback"));
                }
                await UpdateDoctorRating(feedback.DoctorId);
                return Ok(_mapper.Map<Feedback, FeedbackInfoDto>(feedback));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }

        /****************************************** Get Feedback By Id ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("get-feedback/{Id:int}")]
        public async Task<ActionResult<FeedbackInfoDto>> GetFeedbackById(int Id)
        {
            var feedback = await _unitOfWork.Repository<Feedback>().GetAsync(Id);
            if(feedback is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Feedback Not Found"));
            }

            var patientId = int.Parse(User.FindFirstValue(Identifiers.PatientId));

            if(feedback.PatientId != patientId)
            {
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Unauthorized"));
            }

            return Ok(_mapper.Map<Feedback, FeedbackInfoDto>(feedback));
        }

        /****************************************** Get Current Patient Feedbacks With Specific Doctor ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("Get-Feedbacks-with-Specific-Doctor/{Id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<ActionResult<IReadOnlyList<FeedbackWithIdToReturnDto>>> GetCurrentPatientReviews(int Id)
        {
            // Get Id From Token Claims
            var patientId = int.Parse(User.FindFirstValue(Identifiers.PatientId));

            // specs to get the Feedbacks where (docId , patientId)
            var spec = new FeedBacksBetweenPatientAndDoctorSpecs(Id,patientId);
            var feedbacks = await _unitOfWork.Repository<Feedback>().GetAllWithSpecAsync(spec);
            if (feedbacks is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "FeedBacks Not Found"));

            return Ok(_mapper.Map<IEnumerable<Feedback>, IReadOnlyList<FeedbackWithIdToReturnDto>>(feedbacks));
        }

        /****************************************** Get All Reviews ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("GetReviews/{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<ActionResult<IReadOnlyList<FeedbackToReturnDto>>> GetDoctorReviews(int id)
        {
            //Handle Specs for reviews 
            var specs = new FeedBackSpecs(id);

            //Fetch reviews of doctor & check if exist or not
            var reviews = await _unitOfWork.Repository<Feedback>().GetAllWithSpecAsync(specs);
            if (reviews is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            // mapping
            var feedbacksToReturnDto = new List<FeedbackToReturnDto>();
            // mapping to dto (FeedBack Attributes only) 
            feedbacksToReturnDto = _mapper.Map(reviews, feedbacksToReturnDto);

            return Ok(feedbacksToReturnDto);
        }

        /****************************************** Delete Feedback ******************************************/
        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpDelete("delete-feedback/{Id:int}")]
        public async Task<ActionResult> DeleteFeedback(int Id)
        {
            var feedback = await _unitOfWork.Repository<Feedback>().GetAsync(Id);
            if (feedback is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Feedback Not Found"));
            }

            var patientId = int.Parse(User.FindFirstValue(Identifiers.PatientId));

            if(feedback.PatientId != patientId)
            {
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Unauthorized"));
            }

            try
            {
                _unitOfWork.Repository<Feedback>().Delete(feedback);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to delete Feedback"));
                }
                await UpdateDoctorRating(feedback.DoctorId);
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Feedback Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }

        }

        /****************************************** Update Doctor Rating ******************************************/
        private async Task<ActionResult> UpdateDoctorRating(int doctorId)
        {
            // 1- get the doctor of this appId
            //var spec = new DoctorForProfileSpecs(doctorId);
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(doctorId);
            if (doctor is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Doctor Not Found"));

            // 2- get all feedbacks of this doctor with the selector for scores
            var doctorSpec = new DoctorWithFeedbackSpecs(doctorId);
            var feedbackScores = await _unitOfWork.Repository<Feedback>().GetAllWithSpecAsync(doctorSpec, f => f.Score);
            if (feedbackScores is null || !feedbackScores.Any())
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Feedback Not Found"));
            }

            // 3- calculate the average of the feedback scores
            var averageScore = feedbackScores.Average();

            // 4- update the doctor rating
            doctor.Rating = averageScore;
            _unitOfWork.Repository<Doctor>().Update(doctor);
            await _unitOfWork.CompleteAsync();

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Doctor Rating Updated Successfully"));
        }
    }
}

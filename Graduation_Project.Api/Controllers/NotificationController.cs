using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Api.DTO.Notification;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.Specifications.NotificationSpecifications;
using Graduation_Project.Service.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Graduation_Project.Api.Controllers
{
    public class NotificationController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetNotifications")]
        public async Task<ActionResult<List<Notification>>> GetAllNotifications()
        {
            // Get the current user id 
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            // load its notifications 
            var notificationSpecs = new AllNotificationsSpecification(userId);
            var notificationsRecipients = await _unitOfWork.Repository<NotificationRecipient>().GetAllWithSpecAsync(notificationSpecs);
            if (notificationsRecipients is null)
                return Ok(new ApiResponse(StatusCodes.Status204NoContent,"There is no notifications !!"));

            // mapping 
            var notifications = _mapper.Map<IReadOnlyList<NotificationRecipient>,IReadOnlyList<NotificationDto>>(notificationsRecipients);
            
            // return it
            return Ok(notifications);
        }
    }
}

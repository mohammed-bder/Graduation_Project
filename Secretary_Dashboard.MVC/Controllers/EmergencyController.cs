using Graduation_Project.Core;
using Graduation_Project.Core.Enums;
using Graduation_Project.Core.Models.Clinics;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Notifications;
using Graduation_Project.Core.Models.Patients;
using Graduation_Project.Core.Specifications.SecretarySpecifications;
using Graduation_Project.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Secretary_Dashboard.MVC.Controllers
{
    public class EmergencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmergencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult index()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmergencyNotice(string message, string recipientGroup)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Message cannot be empty.";
                return RedirectToAction("index");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            // إحضار المرضى حسب الاختيار
            List<Patient> patients;

            if (recipientGroup == "Today's Patients")
            {
                var appointmentsSpec = new AppointmentsWithPatientsForTodaySpecification(today);
                var appointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(appointmentsSpec);

                patients = appointments
                    .Select(a => a.Patient)
                    .Where(p => p != null)
                    .DistinctBy(p => p.Id)
                    .ToList();
            }
            else if (recipientGroup == "All Patients")
            {
                patients = (List<Patient>)await _unitOfWork.Repository<Patient>().GetAllAsync();
            }
            else
            {
                TempData["Error"] = "Please select a valid recipient group.";
                return RedirectToAction("emergency");
            }

            // إنشاء الإشعار
            var notification = new Notification
            {
                Title = "🚨 Emergency Alert",
                Message = message,
                CreatedDate = DateTime.Now,
                Recipients = patients.Select(p => new NotificationRecipient
                {
                    RecipientType = RecipientType.Patient,
                    IsRead = false,
                    UserId = null // لأنه مش مرتبط بـ AppUser
                }).ToList()
            };

            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            TempData["Success"] = "Emergency notification sent successfully.";
            return RedirectToAction("index");
        }





    }
}

using FirebaseAdmin.Messaging;
using Graduation_Project.Core;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Clinics;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Notifications;
using Graduation_Project.Core.Specifications.AppointmentSpecs;
using Graduation_Project.Core.Specifications.SecretarySpecifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Secretary_Dashboard.MVC.Controllers
{
    public class QueueController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly INotificationService _notificationService;

        public QueueController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager , INotificationService notificationSERVICE)
        {
            _unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _notificationService = notificationSERVICE;
        }
        public async Task<IActionResult> index(DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;
            var dateOnly = DateOnly.FromDateTime(targetDate);

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var id = user.Id; // from identity table

            var spec = new SecretaryByAppuserIdSpecification(id);
            var secretary = await _unitOfWork.Repository<Secretary>().GetWithSpecsAsync(spec);

            var doctor = secretary.clinic.Doctor;



            // Get Today Appointments For Table 
            var specs = new AllDoctorGenericAppointmentsSpecification(doctor.Id, targetDate);

            var Todayappointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(specs); // included with patients details

            ViewBag.SelectedDate = targetDate;

            return View(Todayappointments);
        }

        public async Task<IActionResult> MarkAsCompleted(int AppointmentId)
        {
            //Get Appointment From DB included with patient
            var PatientAppointmentForSpecificDoctorSPEC = new PatientAppointmentForSpecificDoctorSpecification(AppointmentId);

            Appointment ConfirmedAppointment = await _unitOfWork.Repository<Appointment>().GetWithSpecsAsync(PatientAppointmentForSpecificDoctorSPEC);
            // change Status of appointment and save into DB 
            ConfirmedAppointment.Status = AppointmentStatus.Completed;
            //var CompletedAppointment = ConfirmedAppointment;
            // Save to DB
            await _unitOfWork.Repository<Appointment>().SaveAsync();
            return RedirectToAction("index");
        }


        public async Task<IActionResult> SendReminder(string ApplicationUserId)
        {

            if (ApplicationUserId == null)
            {
                TempData["Error"] = "Patient information not found.";
                return RedirectToAction("Index");
            }


            await _notificationService.SendNotificationAsync(ApplicationUserId, "Get Ready, Your Turn Is Close", "Appointment Reminder");
            TempData["Success"] = "Notification sent successfully.";
            return RedirectToAction("index");
        }


        public async Task<IActionResult> Cancel(int AppointmentId)
        {

            var _Appoinmtent = await _unitOfWork.Repository<Appointment>().GetAsync(AppointmentId);

            if (_Appoinmtent is null)
            {
                TempData["Error"] = "No confirmed appointment found for today.";
                return RedirectToAction("Index");
            }
            _Appoinmtent.Status = AppointmentStatus.Cancelled;

            await _unitOfWork.CompleteAsync();

            TempData["Success"] = "Appointment cancelled successfully.";
            return RedirectToAction("Index");

        }


    }
}

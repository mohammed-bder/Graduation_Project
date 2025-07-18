using Graduation_Project.Core;
using Graduation_Project.Core.Models.Clinics;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Specifications.AppointmentSpecs;
using Graduation_Project.Core.Specifications.SecretarySpecifications;
using Graduation_Project.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Secretary_Dashboard.MVC.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        public AppointmentController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Index(DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;
            var dateOnly = DateOnly.FromDateTime(targetDate);



            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var ApplicationUserid = user.Id; // from identity table

            var spec = new SecretaryByAppuserIdSpecification(ApplicationUserid);
            var secretary = await _unitOfWork.Repository<Secretary>().GetWithSpecsAsync(spec);

            var doctor = secretary.clinic.Doctor;

            // Get Today Appointments For Table
            var specs = new AllDoctorGenericAppointmentsSpecification(doctor.Id, targetDate);

            var appointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(specs); // included with patients details

            ViewBag.SelectedDate = targetDate;
            return View(appointments);
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

using Graduation_Project.Core;
using Graduation_Project.Core.Models.Clinics;
using Graduation_Project.Core.Models.Doctors;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Patients;
using Graduation_Project.Core.Specifications.AppointmentSpecs;
using Graduation_Project.Core.Specifications.SecretarySpecifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Secretary_Dashboard.MVC.Models;
using System.Diagnostics;
using System.Numerics;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Secretary_Dashboard.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public HomeController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
    
        public async Task<IActionResult> index()
        {
            

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user =await userManager.FindByEmailAsync(email);
            var id = user.Id; // from identity table

            var spec = new SecretaryByAppuserIdSpecification(id);
            var secretary = await _unitOfWork.Repository<Secretary>().GetWithSpecsAsync(spec);

            var doctor = secretary.clinic.Doctor;



            // Get Today Appointments For Table
            var specs = new AllDoctorAppointmentsSpecification(doctor.Id);

            var appointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(specs); // included with patients details

            // Get Today ConfirmedAppoitments FOR Queue
            var ConfirmedAppoitmentsSPEC = new AllDoctorAppointmentsConfirmedSpecification(doctor.Id);

            var  OnlyConfirmedAppoitments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(ConfirmedAppoitmentsSPEC);

            ViewData["AllTodayappointments"] = appointments;  // for table of today appointments
            return View(OnlyConfirmedAppoitments); 
        }
        public async Task<IActionResult> Exit(int Id)
        {
            /*

            //Get Appointment From DB
            var PatientAppointmentForSpecificDoctorSPEC = new PatientAppointmentForSpecificDoctorSpecification(Id);

            Appointment ConfirmedAppointment = await _unitOfWork.Repository<Appointment>().GetWithSpecsAsync(PatientAppointmentForSpecificDoctorSPEC);


            // change Status of appointment and save into DB 
            ConfirmedAppointment.Status = AppointmentStatus.Completed;
              var CompletedAppointment = ConfirmedAppointment;
            //await _unitOfWork.Repository<Appointment>().SaveAsync();

            // Add Completed appointment to CompletedPatientsList(Previous List)
            List<Appointment> CompletedPatientsList = new List<Appointment>();

            CompletedPatientsList.Add(CompletedAppointment);  //have complete appointment come from view 

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            TempData["PreviousAppointments"] = JsonSerializer.Serialize(CompletedPatientsList,options) ;

            List<Appointment> appointments = new();

            if (TempData.ContainsKey("PreviousAppointments"))
            {
                var json = TempData["PreviousAppointments"] as string;

                if (!string.IsNullOrEmpty(json))
                {

                    var option = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };

                     appointments = JsonSerializer.Deserialize<List<Appointment>>(TempData["PreviousAppointments"] as string, option);
                }
            }

            ViewData["PreviousAppointments"] = appointments;

            return RedirectToAction("index");

            */
            //*********************************************//
            //Get Appointment From DB included with patient
            var PatientAppointmentForSpecificDoctorSPEC = new PatientAppointmentForSpecificDoctorSpecification(Id);
            
            Appointment ConfirmedAppointment = await _unitOfWork.Repository<Appointment>().GetWithSpecsAsync(PatientAppointmentForSpecificDoctorSPEC);
            // change Status of appointment and save into DB 
            ConfirmedAppointment.Status = AppointmentStatus.Completed;
            var CompletedAppointment = ConfirmedAppointment;
            // Save to DB
            await _unitOfWork.Repository<Appointment>().SaveAsync();
            return RedirectToAction("index");
        }


    }
}

using Microsoft.AspNetCore.Mvc;

namespace Secretary_Dashboard.MVC.Controllers
{
    public class EmergencyController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SendEmergencyNotice(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                // Store it in DB or send it via notifications
                //_notificationService.BroadcastToWaitingPatients(message);

                TempData["Success"] = "Emergency message sent.";
            }
            return RedirectToAction("Queue"); // or wherever you show the queue
        }



    }
}

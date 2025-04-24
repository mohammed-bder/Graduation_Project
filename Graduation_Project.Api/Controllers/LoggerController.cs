using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{

    public class LoggerController : BaseApiController
    {
        [HttpGet("Last-Log")]   
        public IActionResult GetLog()
        {
            var logDir = Path.Combine(AppContext.BaseDirectory, "Logs");
            if (!Directory.Exists(logDir))
                return NotFound(new { message = "Log folder does not exist." });

            var latestLog = Directory.GetFiles(logDir, "app*.log")
                                     .OrderByDescending(f => f)
                                     .FirstOrDefault();

            if (latestLog == null)
                return NotFound("No log file found.");

            var content = System.IO.File.ReadAllText(latestLog);
            return Content(content, "text/plain");
        }
    }
}

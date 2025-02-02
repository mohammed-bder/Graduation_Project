using Graduation_Project.Api.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers
{
    public class BuggyController : BaseApiController
    {
        // This controller is used to test the error handling middleware

        //1- NotFound
        [HttpGet("NotFound")]
        public ActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }

        //2- Bad Request
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        //3- unauthorized
        [HttpGet("UnAuthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }

        //4- validation error
        [HttpGet("ValidationError/{id}")]
        public ActionResult GetValidationError(int id)
        {
            // the user enter string instead of int
            return Ok();
        }

        //5- Server Error
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            object obj = null;
            obj.ToString();
            return Ok(obj);
        }

        //6- Endpoint not found

    }
}

using Graduation_Project.Core.IServices;
using Graduation_Project.Core;
using Graduation_Project.Core.Specifications.FavouriteSpecifications;
using Graduation_Project.Core.Specifications.PatientSpecifications;
using Graduation_Project.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Api.Attributes;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Api.Filters;

namespace Graduation_Project.Api.Controllers.DoctorControllers
{
    public class FavouriteController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;

        public FavouriteController(IUnitOfWork unitOfWork
                                , IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<ActionResult<bool>> GetFavourite(int id)
        {
            // get current patient 
            var user = await userService.GetCurrentUserAsync();
            var patientSpecs = new PatientForProfileSpecs(user.Id);
            var patient = await unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpecs);

            // get favourite by (doctorId , patientId)
            // check if is it exist or not 
            var favouriteSpecs = new FavouriteSpecs(id, patient.Id);
            var favouriteDoctor = await unitOfWork.Repository<Favorite>().GetWithSpecsAsync(favouriteSpecs);
            if (favouriteDoctor == null)
                return Ok(false);

            return Ok(true);
        }

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpPost("{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<ActionResult<bool>> AddFavourite(int id)
        {
            // get current patient 
            var user = await userService.GetCurrentUserAsync();
            var patientSpecs = new PatientForProfileSpecs(user.Id);
            var patient = await unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpecs);

            // get favourite to this patientId , DoctorId to check if this is already exist or not
            var specs = new FavouriteSpecs(id, patient.Id);
            var favouriteFromDb = await unitOfWork.Repository<Favorite>().GetWithSpecsAsync(specs);
            if (favouriteFromDb != null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Doctor is already Favourite for the current patient"));

            // add favourite record
            var favourite = new Favorite
            {
                DoctorId = id,
                PatientId = patient.Id,
            };
            await unitOfWork.Repository<Favorite>().AddAsync(favourite);

            return Ok(new ApiResponse(StatusCodes.Status201Created,"Created Successfully"));
        }

        [Authorize(Roles = nameof(UserRoleType.Patient))]
        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ExistingIdFilter<Doctor>))]
        public async Task<ActionResult<bool>> RemoveFavourite(int id)
        {
            // get current patient 
            var user = await userService.GetCurrentUserAsync();
            var patientSpecs = new PatientForProfileSpecs(user.Id);
            var patient = await unitOfWork.Repository<Patient>().GetWithSpecsAsync(patientSpecs);

            // get favourite to this patientId , DoctorId to check if this is already exist or not
            var specs = new FavouriteSpecs(id, patient.Id);
            var favourite = await unitOfWork.Repository<Favorite>().GetWithSpecsAsync(specs);
            if (favourite == null)
                return NotFound(new ApiResponse(StatusCodes.Status400BadRequest, "Doctor is not Favourite for the current patient"));
            // add favourite record

            unitOfWork.Repository<Favorite>().Delete(favourite);
            await unitOfWork.Repository<Favorite>().SaveAsync();

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Removed Successfully"));
        }
    }
}

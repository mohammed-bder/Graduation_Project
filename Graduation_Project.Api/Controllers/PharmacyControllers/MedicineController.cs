using Graduation_Project.Api.DTO.Pharmacies;
using Graduation_Project.Api.ErrorHandling;
using Graduation_Project.Core;
using Graduation_Project.Core.Specifications.MedicineSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Api.Controllers.PharmacyControllers
{
    public class MedicineController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //[Authorize(Roles = nameof(UserRoleType.Doctor))]
        [HttpGet("GetMedicineByName")]
        public async Task<ActionResult<IReadOnlyList<MedicinesForSearchResultDto>>> GetMedicinesByName([FromQuery] string? name, [FromQuery] int count = 20)// count will increase with every showMore
        {
            // check on name
            if (string.IsNullOrEmpty(name))
                return BadRequest(new ApiResponse(400, "Please enter a medicine name"));

            // Get Matched medicines
            var spec = new MedicineSpec(name, count);
            var matchedMedicines =
                        await _unitOfWork.Repository<Medicine>().GetAllWithSpecAsync(spec,m => new MedicinesForSearchResultDto {Id = m.Id, Name =m.Name_en });

            if (matchedMedicines.IsNullOrEmpty())
                return NotFound(new ApiResponse(404, "No medicines found with this name"));

            // mapping
            return Ok(matchedMedicines);
        }
    }
}

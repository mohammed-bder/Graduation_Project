using AutoMapper;
using Graduation_Project.Core;
using Graduation_Project.Core.Constants;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Core.Models.SendingEmail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_Dashboard.MVC.ViewModel.Account;
using System.Security.Claims;


namespace Pharmacy_Dashboard.MVC.Controllers
{
    public class PharmacistController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public PharmacistController(IUnitOfWork unitOfWork , UserManager<AppUser> userManager , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        /****************************************** Edit Profile Info ******************************************/
        public async Task<IActionResult> EditProfile()
        {
            var pharmacistId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(pharmacistId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch pharmacist details from the database using pharmacistId
            var pharmacy = await _unitOfWork.Repository<Pharmacy>().GetByConditionAsync(p => p.ApplicationUserID == pharmacistId);
            var pharmacyContacts = await _unitOfWork.Repository<PharmacyContact>().GetManyByConditionAsync(p => p.PharmacyId == pharmacy.Id);


            var model = _mapper.Map<Pharmacy, EditProfileViewModel>(pharmacy);
            model.Email = User.FindFirstValue(ClaimTypes.Email);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pharmacistId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Fetch the existing pharmacy from the database
                var pharmacy = await _unitOfWork.Repository<Pharmacy>().GetByConditionAsync(p => p.ApplicationUserID == pharmacistId);
                if (pharmacy == null)
                {
                    ModelState.AddModelError(string.Empty, "Pharmacy not found.");
                    return View(model);
                }

                // Update pharmacy details
                pharmacy.Name = model.PharmacyName;
                pharmacy.Address = model.Address;
                pharmacy.Latitude = model.Latitude ?? pharmacy.Latitude;
                pharmacy.Longitude = model.Longitude ?? pharmacy.Longitude;


                // Save changes
                _unitOfWork.Repository<Pharmacy>().Update(pharmacy);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update profile. Please try again.");
                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}

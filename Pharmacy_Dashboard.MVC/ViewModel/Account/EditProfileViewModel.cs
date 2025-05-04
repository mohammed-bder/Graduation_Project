using Graduation_Project.Core.Models.Pharmacies;

namespace Pharmacy_Dashboard.MVC.ViewModel.Account
{
    public class EditProfileViewModel
    {
        public string Email { get; set; }
        public string PharmacyName { get; set; }
        public List<PharmacyContactViewModel>? PharmacyContacts { get; set; } = new();
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? PictureUrl { get; set; }

    }
}

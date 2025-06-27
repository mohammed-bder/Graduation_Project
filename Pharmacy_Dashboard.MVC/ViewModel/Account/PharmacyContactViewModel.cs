using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModel.Account
{
    public class PharmacyContactViewModel
    {
        public int Id { get; set; }

        [RegularExpression(@"^\+?\d{10,11}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }
    }
}

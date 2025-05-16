using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModel.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModel.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Minimum Password Length is 6")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,10}$",
    ErrorMessage = "Password must be between 8 and 15 characters and contain at least one lowercase letter," +
    " one uppercase letter, one digit and one special character.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password does not match Password ")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}

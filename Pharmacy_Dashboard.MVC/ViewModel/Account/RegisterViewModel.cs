using Graduation_Project.Core.Models.Pharmacies;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModel.Account
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(6,ErrorMessage = "Minimum Password Length is 6")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,10}$",
			ErrorMessage = "Password must be between 8 and 15 characters and contain at least one lowercase letter," +
			" one uppercase letter, one digit and one special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
		[Compare(nameof(Password) ,ErrorMessage = "Confirm Password does not match Password "  )]
		[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Pharmacy name is required.")]
		[StringLength(100, ErrorMessage = "Pharmacy name cannot exceed 100 characters.")]
		public string PharmacyName { get; set; }

		[Required(ErrorMessage = "Pharmacy address is required.")]
        public string Address { get; set; }

		
		[Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
		public double Latitude { get; set; }

		[Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
		public double Longitude { get; set; }

		[Required(ErrorMessage = "Pharmacy license Image is required.")]
		[DataType(DataType.Upload)]
		public string? LicenseImageUrl { get; set; }

		//[Phone(ErrorMessage = "Please enter a valid contact number.")]
		//public List<PharmacyContact>? pharmacyContacts { get; set; }

	}
}

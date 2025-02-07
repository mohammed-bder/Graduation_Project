using AutoMapper.Configuration.Conventions;
using System.ComponentModel.DataAnnotations;

namespace Talabat.API.Dtos.Account
{
    public class DoctorRegisterDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name should Contain at least 3 characters")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,10}$",
            ErrorMessage = "Password must be between 8 and 15 characters and contain at least one lowercase letter," +
            " one uppercase letter, one digit and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        public int SpecialtyId { get; set; }

        //[Required]
        //public string PhoneNumber { get; set; }



        //[StringLength(14, ErrorMessage = "National ID must be 14 characters.")]
        //public string? NationalID { get; set; }


        public string? MedicalLicensePictureUrl { get; set; }



        //[Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        //public double? Rating { get; set; }


        [Required(ErrorMessage = "Consultation fees are required.")]
        [Range(0.0, 10000.0, ErrorMessage = "Consultation fees must be a positive number.")]
        public decimal ConsultationFees { get; set; }
    }
}

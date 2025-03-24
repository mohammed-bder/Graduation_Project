using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Clinics
{
    public class ContactNumberDTO
    {

        [Required(ErrorMessage = "Phone number is required. Please enter a valid phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number format.")]
        public string PhoneNumber { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO
{
    public class PersonToReturnDTO
    {
        public int Id { get; set; }

        //[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

     
        //[StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string? LastName { get; set; }

     
        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }


        public string? PictureUrl { get; set; }
     

    }
}

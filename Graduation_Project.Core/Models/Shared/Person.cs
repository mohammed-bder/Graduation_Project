namespace Graduation_Project.Core.Models.Shared
{
    public class Person : BaseEntity
    {

        public string ApplicationUserId { get; set; } // Link to Identity DB

        //[Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string? LastName { get; set; }

        //[Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        public string? PictureUrl { get; set; }



    }
}

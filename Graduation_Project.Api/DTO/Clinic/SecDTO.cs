namespace Graduation_Project.Api.DTO.Clinic
{
    public class SecDTO
    {
        public int Id { get; set; } // Primary key for the secretary entity

        //[Required(ErrorMessage = "National ID is required.")]
        [StringLength(14, ErrorMessage = "National ID must be 14 characters.")]
        public string? NationalID { get; set; } // National ID of the pharmacist

        //[Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string? LastName { get; set; }

        //[Required(ErrorMessage = "Date of birth is required.")]
        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        //public string Image { get; set; }
        public byte[]? ImageImgData { get; set; }

    }
}

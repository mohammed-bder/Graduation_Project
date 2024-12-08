namespace Graduation_Project.Api.DTO.Account
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is Required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is Required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Role is Required.")]
        public string Role { get; set; }
    }
}

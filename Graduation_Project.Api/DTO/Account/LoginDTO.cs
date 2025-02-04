using System.ComponentModel.DataAnnotations;

namespace Talabat.API.Dtos.Account
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

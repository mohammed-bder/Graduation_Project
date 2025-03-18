namespace Graduation_Project.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } // Store full name for quick access
                                             //public UserType UserType { get; set; } // Enum: Doctor or Patient
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

    }
}

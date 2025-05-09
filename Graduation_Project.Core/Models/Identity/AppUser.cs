namespace Graduation_Project.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } 
        public List<RefreshToken>? RefreshTokens { get; set; }
        public string? DeviceToken { get; set; }
    }
}

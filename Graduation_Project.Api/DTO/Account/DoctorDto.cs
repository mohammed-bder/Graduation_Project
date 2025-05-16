using System.Text.Json.Serialization;

namespace Graduation_Project.Api.DTO.Account
{
    public class DoctorDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }

        public string Speciality { get; set; }

        public string? Description { get; set; }

        public string? PictureUrl { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

    }
}

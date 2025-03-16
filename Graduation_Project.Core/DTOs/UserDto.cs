using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graduation_Project.Core.DTOs
{
    public class UserDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string? PictureUrl { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }

        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}

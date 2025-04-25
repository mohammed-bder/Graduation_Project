using AutoMapper;
using Graduation_Project.Api.DTO.Pharmacies;

namespace Graduation_Project.Api.Helpers
{
    public class PharmacyPictureUrlResolver : IValueResolver<PharmacyWithDistanceDTO, PharmacyCardDTO, string>
    {
        private readonly IConfiguration _configuration;

        public PharmacyPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(PharmacyWithDistanceDTO source, PharmacyCardDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.pharmacy.ProfilePictureUrl))
            {
                return source.pharmacy.ProfilePictureUrl[0] == '/' ? $"{_configuration["ServerUrl"]}{source.pharmacy.ProfilePictureUrl}" : $"{_configuration["ServerUrl"]}/{source.pharmacy.ProfilePictureUrl}";
            }
            return null;
        }
    }
}


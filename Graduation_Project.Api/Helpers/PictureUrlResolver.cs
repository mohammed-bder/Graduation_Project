using AutoMapper;
using Graduation_Project.Core.Models;

namespace Graduation_Project.Api.Helpers
{
    public class PictureUrlResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
        where TSource : BaseEntity
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
        {
            // Get the PictureUrl property dynamically
            var pictureUrlProperty = typeof(TSource).GetProperty("PictureUrl");
            var MedicalLicensePictureUrlProperty = typeof(TSource).GetProperty("MedicalLicensePictureUrl");

            if (pictureUrlProperty != null)
            {
                var pictureUrl = pictureUrlProperty.GetValue(source) as string;
                if (!string.IsNullOrEmpty(pictureUrl))
                    return pictureUrl[0] == '/' ? $"{_configuration["ServerUrl"]}{pictureUrl}" : $"{_configuration["ServerUrl"]}/{pictureUrl}";
            }

            if (MedicalLicensePictureUrlProperty != null)
            {
                var medicalLicensePictureUrl = MedicalLicensePictureUrlProperty.GetValue(source) as string;
                if (!string.IsNullOrEmpty(medicalLicensePictureUrl))
                    return medicalLicensePictureUrl[0] == '/' ? $"{_configuration["ServerUrl"]}{medicalLicensePictureUrl}" : $"{_configuration["ServerUrl"]}/{medicalLicensePictureUrl}";
            }

            return string.Empty;
        }
    }
}

using AutoMapper;
using Graduation_Project.Api.DTO.Patients;

namespace Graduation_Project.Api.Helpers
{
    public class MedicalHistoryPictureUrlResolver : IValueResolver<MedicalHistory, MedicalHistoryDto, string>
    {
        private readonly IConfiguration _configuration;

        public MedicalHistoryPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(MedicalHistory source, MedicalHistoryDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.MedicalImage))
                return $"{_configuration["ApiBaseUrl"]}/{source.MedicalImage}";

            return string.Empty;
        }
    }
}

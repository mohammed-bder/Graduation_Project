using AutoMapper;
using Graduation_Project.Api.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Graduation_Project.Api.Helpers
{
    public class PersonPictureUrlResolver : IValueResolver<Person, PersonToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public PersonPictureUrlResolver(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        public string Resolve(Person source, PersonToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}";

            return string.Empty;
          
        }
    }
}

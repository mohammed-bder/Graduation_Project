using Graduation_Project.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Clinics
{
    public class GovernorateDTO 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RegionDTO> Regions { get; set; }
    }
}

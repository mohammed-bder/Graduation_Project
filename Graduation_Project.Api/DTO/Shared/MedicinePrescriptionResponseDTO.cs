using Graduation_Project.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Shared
{
    public class MedicinePrescriptionResponseDTO
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string Details { get; set; }
    }
}

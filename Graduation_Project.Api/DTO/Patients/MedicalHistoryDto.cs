using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Graduation_Project.Core.Models.Patients;

namespace Graduation_Project.Api.DTO.Patients
{
    public class MedicalHistoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public string Date { get; set; }
        public string? MedicalImage { get; set; }
    }
}

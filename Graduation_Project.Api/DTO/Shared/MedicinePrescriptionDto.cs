using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Api.DTO.Shared
{
    public class MedicinePrescriptionDto
    {
        public int MedicineId { get; set; }
        public string Details { get; set; }
    }
}

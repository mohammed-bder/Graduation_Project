using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Api.DTO.Pharmacies
{
    public class MedicinePrescription
    {
        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }
        public string Prescription { get; set; }

        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        public string Medicine { get; set; }

        public int Details { get; set; }
    }
}

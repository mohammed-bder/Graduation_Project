using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Pharmacies
{
    public class OrderDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "PharmacyId must be greater than 0.")]
        public int PharmacyId { get; set; }

        [Required]
        [Phone]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 characters.")]
        public string PatientPhoneNumber { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
        public string PatientAddress { get; set; }

        [Required(ErrorMessage = "MedicinesDictionary is required.")]
        public Dictionary<int, int>? MedicinesDictionary { get; set; }
    }

}

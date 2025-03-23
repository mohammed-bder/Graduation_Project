namespace Graduation_Project.Core.Models.Clinics
{
    public class ContactNumber : BaseEntity
    {
        //Composite Key By Fluent Api ( ClinicId , Number )
        [ForeignKey("clinic")]
        public int ClinicId { get; set; }
        public Clinic? clinic { get; set; }

        [Required(ErrorMessage = "Phone number is required. Please enter a valid phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number format.")]
        public string PhoneNumber { get; set; }
    }
}

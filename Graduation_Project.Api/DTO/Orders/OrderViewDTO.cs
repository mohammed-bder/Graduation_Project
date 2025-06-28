namespace Graduation_Project.Api.DTO.Orders
{
    public class OrderViewDTO
    {
        public string? PatientPhoneNumber { get; set; }
        public string? PatientAddress { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyAddress { get; set; }
        public List<string> PharmacyPhoneNumber { get; set; }
        public string? PharamcyPictureUrl { get; set; }
    }
}

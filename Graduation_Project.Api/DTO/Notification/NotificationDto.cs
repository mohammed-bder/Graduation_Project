namespace Graduation_Project.Api.DTO.Notification
{
    public class NotificationDto
    {
        public string Title { get; set; } 
        public string Message { get; set; } 
        public DateTime CreatedDate { get; set; }
        public bool? IsRead { get; set; }
    }
}

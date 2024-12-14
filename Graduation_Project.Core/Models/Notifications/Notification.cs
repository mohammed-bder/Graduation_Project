namespace Graduation_Project.Core.Models.Notifications
{
    public class Notification : BaseEntity
    {
        [Required]
        public string Message { get; set; } // The notification message content

        [Required]
        public string Type { get; set; } // Type of the notification (e.g., "Appointment", "Reminder")

        [Required]
        public DateTime CreatedDate { get; set; } // The date and time the notification was created

        //================================Relations==========================
        public ICollection<NotificationRecipient> Recipients { get; set; } // Navigation property for recipients
    }
}

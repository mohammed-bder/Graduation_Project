namespace Graduation_Project.Core.Models.Notifications
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } // Type of the notification (e.g., "Appointment", "Reminder")
        public string Message { get; set; } // The notification message content
        public DateTime CreatedDate { get; set; } // The date and time the notification was created

        //================================Relations==========================
        public ICollection<NotificationRecipient> Recipients { get; set; } // Navigation property for recipients
    }
}

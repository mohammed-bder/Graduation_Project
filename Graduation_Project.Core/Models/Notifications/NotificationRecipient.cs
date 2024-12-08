namespace Graduation_Project.Core.Models.Notifications
{
    public class NotificationRecipient
    {
        [Key]
        public int Id { get; set; } // Primary key for the NotificationRecipient entity

        [Required]
        [StringLength(50)]
        public RecipientType RecipientType { get; set; } // Type of the recipient (e.g., "Doctor", "Patient")

        public bool IsRead { get; set; } // Indicates if the notification has been read
        
        //================================Relations==========================
        //With Notification
        public int NotificationId { get; set; } // Foreign key linking to the Notification

        [ForeignKey("NotificationId")]
        public Notification Notification { get; set; } // Navigation property to Notification

        //With Doctor
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor doctor { get; set; }

        //With Patient
        public int patientId { get; set; }

        [ForeignKey("patientId")]
        public Patient patient { get; set; }

        //With Secretary
        public int secretaryId { get; set; }

        [ForeignKey("secretaryId")]
        public Secretary secretary { get; set; }

        //With Pharmacist
        public int pharmacistId { get; set; }

        [ForeignKey("pharmacistId")]
        public Pharmacist pharmacist { get; set; }

    }
}


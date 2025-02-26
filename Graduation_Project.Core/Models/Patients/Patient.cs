namespace Graduation_Project.Core.Models.Patients
{
    public class Patient: Person
    {
        public int? Points { get; set; }
        public BloodType? BloodType { get; set; }

        //public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<MedicalHistory>? MedicalHistories { get; set; }
        public ICollection<AI_QuickDiagnosis>? AIQuickDiagnosis { get; set; }
        public ICollection<PharmacyOrder>? PharmacyOrder { get; set; }
        public ICollection<NotificationRecipient>? NotificationRecipients { get; set; }
        public ICollection<TherapySession>? TherapySessions { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
        public ICollection<RadiologyReport>? RadiologyReports { get; set; }
    }
}

//using Graduation_Project.Core.Models.Shared;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Graduation_Project.Repository.Data.Configuration.Doctors
//{
//    internal class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
//    {
//        public void Configure(EntityTypeBuilder<Doctor> builder)
//        {
//            builder.HasKey(d => d.Id);

//            builder.Property(d => d.DateOfBirth)
//                   .IsRequired(false);

//            builder.Property(d => d.PhoneNumber)
//                   .IsRequired(false);

//            builder.Property(d => d.Gender)
//                   .IsRequired();

//            builder.Property(d => d.NationalID)
//                   .IsRequired(false)
//                   .HasMaxLength(14)
//                   .HasAnnotation("ErrorMessage", "National ID must be exactly 14 characters.");

//            builder.Property(d => d.MedicalLicenseImgData)
//                   .HasColumnType("varbinary(max)")
//                   .IsRequired(false);

//            builder.Ignore(d => d.MedicalLicenseFile);

//            builder.Property(d => d.Description)
//                   .HasMaxLength(500)
//                   .HasAnnotation("ErrorMessage", "Description cannot exceed 500 characters.")
//                   .IsRequired(false);

//            builder.Property(d => d.Rating)
//                   .IsRequired(false);

//            builder.HasCheckConstraint("CK_Doctor_Rating_Range", "Rating >= 0 AND Rating <= 5");

//            builder.Property(d => d.ConsultationFees)
//                   .IsRequired();

//            builder.HasCheckConstraint("CK_Doctor_ConsultationFees", "ConsultationFees >= 0 AND ConsultationFees <= 10000.0");

//            /********************* Relationships *********************/

//            // Relation between Doctor <=> Subspeciality (M to M)
//            // Relation between Doctor <=> Clinic (M to M)

//            // Relation between Doctor <=> Education (1 to M)
//            builder.HasMany(d => d.Educations)
//                   .WithOne(e => e.Doctor)
//                   .HasForeignKey(e => e.DoctorId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            // Relation between Doctor <=> Specialty (M:1)
//            builder.HasOne(d => d.Specialty)
//                   .WithMany(s => s.Doctors)
//                   .HasForeignKey(d => d.SpecialtyId)
//                   .OnDelete(DeleteBehavior.Restrict); 

//            // Relation between Doctor <=> NotificationRecipient (1:M)
//            builder.HasMany(d => d.NotificationRecipients)
//                   .WithOne(nr => nr.Doctor) 
//                   .HasForeignKey(nr => nr.DoctorId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            // Relation between Doctor <=> TherapySession (1:M)
//            builder.HasMany(d => d.TherapySessions)
//                   .WithOne(ts => ts.Doctor) 
//                   .HasForeignKey(ts => ts.DoctorId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            // Relation between Doctor <=> Appointment (1:M)
//            builder.HasMany(d => d.Appointments)
//                   .WithOne(a => a.Doctor) 
//                   .HasForeignKey(a => a.DoctorId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            // Relation between Doctor <=> Favorite (1:M)
//            builder.HasMany(d => d.Favorites)
//                   .WithOne(f => f.Doctor) 
//                   .HasForeignKey(f => f.DoctorId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            // Relation between Doctor <=> Feedback (1:M)
//            builder.HasMany(d => d.Feedbacks)
//                   .WithOne(f => f.Doctor) 
//                   .HasForeignKey(f => f.DoctorId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            // Relation between Doctor <=> Prescription (1:M)
//            builder.HasMany(d => d.Prescriptions)
//                   .WithOne(p => p.Doctor) 
//                   .HasForeignKey(p => p.DoctorId)
//                   .OnDelete(DeleteBehavior.SetNull);


//        }
//    }
//}

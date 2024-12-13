using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Graduation_Project.Repository.Data.Configuration.Clinics
{
    internal class ClinicConfigurations : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasAnnotation("ErrorMessage", "Please, Enter Your Name");

            builder.Property(c => c.DbImgData)
                .HasColumnType("varbinary(max)")
                .IsRequired(false);

            builder.Ignore(c => c.ImgFile);
                
            builder.Property(c => c.LocationLink)
                .IsRequired(false);

            builder.Property(c => c.Location)
                .IsRequired()
                .HasAnnotation("ErrorMessage", "Location is required");

            builder.Property(c => c.Type)
                .IsRequired()
                .HasAnnotation("ErrorMessage", "Please select a clinic type");

            builder.HasOne(c => c.Region)
                .WithMany()
                .HasForeignKey(c => c.RegionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasAnnotation("ErrorMessage", "Region is required");

            builder.HasMany(c => c.ContactNumbers)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Appointments)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.therapySessions)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.clincSecretarys)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Graduation_Project.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Repository.Data.Configuration.Doctors
{
    internal class EducationConfiguraions : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Degree)
                   .HasAnnotation("ErrorMessage" , "Degree is required." )
                   .IsRequired();

            builder.Property(e => e.Specialty)
                   .HasAnnotation("ErrorMessage", "Specialty is required.")
                   .IsRequired();

            builder.Property(e => e.Institution)
                    .HasMaxLength(100)
                    .HasAnnotation("ErrorMessage", "Institution name cannot exceed 100 characters.")
                   .IsRequired();

            builder.Property(e => e.GraduationDate)
                   .IsRequired();

            /********************* Relationships *********************/

            // Relation between Education <=> Doctor (M to 1)
            builder.HasOne(e => e.Doctor)
                .WithMany(d => d.Educations)
                .HasForeignKey(e => e.Doctor.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

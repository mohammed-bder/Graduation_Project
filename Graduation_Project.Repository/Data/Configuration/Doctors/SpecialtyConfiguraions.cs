using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Repository.Data.Configuration.Doctors
{
    internal class SpecialtyConfiguraions : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasAnnotation("ErrorMessage" , "Specialty name cannot exceed 50 characters.");

            /********************* Relationships *********************/
            // Relation between Specialty <=> SubSpecialities (1 to M)
            builder.HasMany(s => s.SubSpecialities)
                   .WithOne(su => su.Specialty)
                   .HasForeignKey(su => su.SpecialtyId);

            // Relation between Specialty <=> Doctor (1 to M)
            builder.HasMany(s => s.Doctors)
                   .WithOne(d => d.Specialty)
                   .HasForeignKey(d => d.SpecialtyId);
        }
    }
}

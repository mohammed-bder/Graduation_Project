using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Repository.Data.Configuration.Doctors
{
    internal class SubSpecialityConfiguraions : IEntityTypeConfiguration<SubSpecialities>
    {
        public void Configure(EntityTypeBuilder<SubSpecialities> builder)
        {
            builder.HasKey(su => su.Id);

            builder.Property(su => su.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasAnnotation("ErrorMessage", "Subspeciality name cannot exceed 50 characters.");

            /********************* Relationships *********************/
            // Relation between SubSpecialities <=> Doctor (M to M)


            // Relation between SubSpecialities <=> Specialty (M to 1)

            builder.HasOne(su => su.Specialty)
                .WithMany(s => s.SubSpecialities)
                .HasForeignKey(su => su.SpecialtyId);

        }
    }
}

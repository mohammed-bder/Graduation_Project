namespace Graduation_Project.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        /*********************Doctors************************/
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<DoctorClinic> DoctorClinics { get; set; }
        public DbSet<DoctorSubspeciality> DoctorSubspecialities { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<SubSpecialities> SubSpecialities { get; set; }


        /* --------------------- Pharmacies Models -----------------  */
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyOrder> PharmacyOrders { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        // table created from M-M relationship
        public DbSet<MedicinePharmacy> MedicinePharmacies { get; set; }
        public DbSet<MedicinePharmacyOrder> MedicinePharmacyOrders { get; set; }

        /* --------------------- Clinics Models -----------------  */
        public DbSet<Clinic> clinics { get; set; }
        public DbSet<ClincSecretary> clincSecretaries { get; set; }
        public DbSet<ContactNumber> contactNumbers { get; set; }
        public DbSet<Secretary> secretaries { get; set; }
        public DbSet<TherapySession> therapySessions { get; set; }
        public DbSet<Governorate> governorates { get; set; }
        public DbSet<Region> regions { get; set; }


        /* --------------------- Notifications Models -----------------  */
        public DbSet<Notification> notifications { get; set; }
        public DbSet<NotificationRecipient> notificationRecipients { get; set; }

        /*********************Patients************************/
        public DbSet<AI_QuickDiagnosis> AI_QuickDiagnoses { get; set; }
        public DbSet<MedicalCategory> MedicalCategories { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<RadiologyReport> RadiologyReports { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClincSecretary>()
                .HasKey(cs => new { cs.ClincId, cs.SecretaryId });

            modelBuilder.Entity<ContactNumber>()
                .HasKey(cn => new { cn.ClinicId, cn.PhoneNumber });

            modelBuilder.Entity<DoctorClinic>()
                .HasKey(dc => new { dc.DoctorId, dc.ClinicId });

            modelBuilder.Entity<DoctorSubspeciality>()
                .HasKey(ds => new { ds.DoctorId, ds.SubSpecialitiesId });

            modelBuilder.Entity<MedicinePharmacy>()
                .HasKey(ds => new { ds.MedicineId, ds.PharmacyId });

            /************** Relation with Identity Table **************/
            modelBuilder.Entity<Doctor>()
            .HasOne(d => d.ApplicationUser)
            .WithOne()
            .HasForeignKey<Doctor>(d => d.UserId);

            modelBuilder.Entity<Patient>()
            .HasOne(d => d.ApplicationUser)
            .WithOne()
            .HasForeignKey<Patient>(d => d.UserId);

            modelBuilder.Entity<Pharmacist>()
            .HasOne(d => d.ApplicationUser)
            .WithOne()
            .HasForeignKey<Pharmacist>(d => d.UserId);

            modelBuilder.Entity<Secretary>()
            .HasOne(d => d.ApplicationUser)
            .WithOne()
            .HasForeignKey<Secretary>(d => d.UserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}


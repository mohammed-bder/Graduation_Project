namespace Graduation_Project.Core.Models.Clinics
{
    public class ClincSecretary
    {
        [ForeignKey("clinic")]
        public int ClincId { get; set; }
        public Clinic clinic { get; set; }

        [ForeignKey("secretary")]
        public int SecretaryId { get; set; }
        public Secretary secretary { get; set; }


    }
}

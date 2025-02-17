using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.DTO.Patients
{
    public class FeedbackInfoDto
    {
        public string Comment { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}

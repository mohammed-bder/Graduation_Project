namespace Graduation_Project.Api.DTO.FeedBacks
{
    public class FeedbackToReturnDto
    {
        public string FullName { get; set; }
        public int? Age { get; set; }
        public string Comment { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
    }
}

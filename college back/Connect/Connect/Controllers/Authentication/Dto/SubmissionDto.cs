namespace Connect.Controllers.Authentication.Dto
{
    public class SubmissionDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string FileUrl { get; set; }
        public DateTime SubmittedOn { get; set; }
    }
}

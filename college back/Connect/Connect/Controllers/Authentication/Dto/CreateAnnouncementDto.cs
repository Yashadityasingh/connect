namespace Connect.Controllers.Authentication.Dto
{
    public class CreateAnnouncementDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? Category { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.UtcNow;
    }
}

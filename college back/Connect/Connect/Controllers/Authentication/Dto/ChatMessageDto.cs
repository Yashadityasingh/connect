namespace Connect.Controllers.Authentication.Dto
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int SenderId { get; set; }
        public string SenderUser { get; set; }
        public string? Text { get; set; }
        public DateTime SentOn { get; set; }
        public string? ImageUrl { get; set; }
    }
}

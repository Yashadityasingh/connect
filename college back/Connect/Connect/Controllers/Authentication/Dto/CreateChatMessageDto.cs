namespace Connect.Controllers.Authentication.Dto
{
    public class CreateChatMessageDto
    {
        public int GroupId { get; set; }
        public string? Text { get; set; }
        public IFormFile? Image { get; set; }
    }
}

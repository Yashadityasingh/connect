namespace Connect.Controllers.Authentication.Dto
{
    public class RegisterDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}

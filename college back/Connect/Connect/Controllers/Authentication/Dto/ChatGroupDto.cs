namespace Connect.Controllers.Authentication.Dto
{
    public class ChatGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Class, Department, Club, etc.
        public int CreatedById { get; set; }
        public string CreatedByUser { get; set; }
    }
}

namespace Connect.Controllers.Authentication.Dto
{
    public class AnnouncementsDto
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public int PostedById { get; set; }
        public string PostedByName { get; set; }
        public string? Category { get; set; }
            public DateTime PostedOn { get; set; }
        }
    }


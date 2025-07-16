namespace Connect.Controllers.Authentication.Dto
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByUser { get; set; }
        public string TargetAudienceType { get; set; } // All, Role, Group
        public int? SpecificGroupId { get; set; }
    }
}

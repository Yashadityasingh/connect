namespace Connect.Controllers.Authentication.Dto
{
    public class CreateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string TargetAudienceType { get; set; }
        public int? SpecificGroupId { get; set; }
    }
}

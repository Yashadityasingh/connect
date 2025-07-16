namespace Connect.Controllers.Authentication.Dto
{
    public class AssigmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int CreatedById { get; set; }
    }
}

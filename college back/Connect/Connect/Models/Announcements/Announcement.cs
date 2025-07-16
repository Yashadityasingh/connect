using System;
using System.ComponentModel.DataAnnotations.Schema;
using api.models;
using api.models.User;


namespace api.models.Announcements
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        // —— ForeignKey to Users.Id (who posted) ——
        [ForeignKey(nameof(PostedBy))]
        public int PostedById { get; set; }
        public Users PostedBy { get; set; }

        public string Category { get; set; } // e.g. "Exam", "Event", "General"
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public string PostedByName { get; set; }
    }
}

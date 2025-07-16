using System;
using System.ComponentModel.DataAnnotations.Schema;
using api.models.User;
using api.models.Chat;

namespace api.models.Events
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }

        // —— ForeignKey to Users.Id (who created) ——
        [ForeignKey(nameof(CreatedBy))]
        public int CreatedById { get; set; }
        public Users CreatedBy { get; set; }

        public string TargetAudienceType { get; set; } // "All", "Role", or "Group"

        // If targeting a specific group, tie it here
        [ForeignKey(nameof(SpecificGroup))]
        public int? SpecificGroupId { get; set; }
        public ChatGroup SpecificGroup { get; set; }
    }
}

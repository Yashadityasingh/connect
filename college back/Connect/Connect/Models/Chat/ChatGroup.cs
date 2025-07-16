using api.models.Events;
using api.models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace api.models.Chat
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }  // e.g. "CS101 Discussion"
        public string Type { get; set; }  // e.g. "Class", "Club"

        // —— ForeignKey to Users.Id (who created this group) ——
        [ForeignKey(nameof(CreatedBy))]
        public int CreatedById { get; set; }
        public Users CreatedBy { get; set; }

        // Many‐to‐many: ChatGroup ↔ Users via GroupMember
        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();

        // One‐to‐many: ChatGroup → ChatMessages
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<Event> Events { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;
using api.models;
using api.models.User;
using api.models.Chat;
using api.models.Announcements;
using api.models.Assignment;
using api.models.Events;
namespace api.models.Chat
{
    public class ChatMessage
    {
        public int Id { get; set; }

        // —— ForeignKey to ChatGroup.Id ——
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public ChatGroup Group { get; set; }

        // —— ForeignKey to Users.Id (the sender) ——
        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }
        public Users Sender { get; set; }

        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime SentOn { get; set; } = DateTime.Now;
    }
}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.models.Chat;
using api.models.Announcements;
using api.models.Assignment;
using api.models.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.models.User
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        [ForeignKey("Roles")]
        public int RoleId { get; set; }

        // A user can have multiple roles (via UserRole)
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // A user can post many announcements
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

        // A user (teacher) can create many assignments
        public ICollection<Assignments> AssignmentsCreated { get; set; } = new List<Assignments>();

        // A user (student) can submit many submissions
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();

        // A user (teacher/admin) can create many events
        public ICollection<Event> EventsCreated { get; set; } = new List<Event>();
        public ICollection<GroupMember> GroupMemberships { get; set; }
        public ICollection<ChatGroup> CreatedChatGroups { get; set; }

        // A user can send many chat messages
        public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();

        // A user can belong to many chat groups
        public ICollection<GroupMember> GroupMembership { get; set; } = new List<GroupMember>();
    }
}

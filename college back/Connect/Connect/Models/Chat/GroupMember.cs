using System.ComponentModel.DataAnnotations.Schema;
using api.models.User;
namespace api.models.Chat
{
    public class GroupMember
    {
        public int Id { get; set; }

        // —— ForeignKey to Users.Id ——
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public Users User { get; set; }

        // —— ForeignKey to ChatGroup.Id ——
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public ChatGroup Group { get; set; }
    }
}

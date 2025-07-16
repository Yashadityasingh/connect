using System.ComponentModel.DataAnnotations.Schema;
using api.models;
using api.models.User;

namespace api.models
{
    public class UserRole
    {
        public int Id { get; set; }

        // —— ForeignKey to Users.Id ——
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public Users User { get; set; }

        // —— ForeignKey to Role.Id ——
        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

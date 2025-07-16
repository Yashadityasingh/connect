
using System.Collections.Generic;

namespace api.models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g. "Student", "Teacher", "Admin"

        // Many‐to‐many: Role ↔ Users via UserRole
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using api.models;
using api.models.User;

namespace api.models.Assignment
{
    public class Assignments
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        // —— ForeignKey to Users.Id (who created) ——
        [ForeignKey(nameof(CreatedBy))]
        public int CreatedById { get; set; }
        public Users CreatedBy { get; set; }

        // One‐to‐many: Assignment → Submission
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}

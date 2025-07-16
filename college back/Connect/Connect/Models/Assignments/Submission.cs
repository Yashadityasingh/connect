using System;
using System.ComponentModel.DataAnnotations.Schema;
using api.models;
using api.models.User;
namespace api.models.Assignment
{
    public class Submission
    {
        public int Id { get; set; }

        // —— ForeignKey to Assignment.Id ——
        [ForeignKey(nameof(Assignment))]
        public int AssignmentId { get; set; }
        public Assignments Assignment { get; set; }

        // —— ForeignKey to Users.Id (the student who submitted) ——
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Users Student { get; set; }

        public string FileUrl { get; set; }
        public DateTime SubmittedOn { get; set; } = DateTime.Now;
        public int? Marks { get; set; }
    }
}

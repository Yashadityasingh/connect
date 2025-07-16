using api.Data;
using api.models.Assignment;
using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class SubmissionService : ISubmissionService
{
    private readonly ConnectDbContext _context;

    public SubmissionService(ConnectDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
    {
        return await _context.Submissions
            .Where(s => s.AssignmentId == assignmentId)
            .Include(s => s.Student)
            .Select(s => new
            {
                s.Id,
                s.AssignmentId,
                Student = s.Student.username,
                s.FileUrl,
                s.SubmittedOn,
                s.Marks
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteSubmissionAsync(int submissionId)
    {
        var submission = await _context.Submissions.FindAsync(submissionId);
        if (submission == null) return false;

        _context.Submissions.Remove(submission);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<object>> GetAllSubmissionsAsync()
    {
        return await _context.Submissions
            .Include(s => s.Student)
            .Include(s => s.Assignment)
            .Select(s => new
            {
                s.Id,
                s.AssignmentId,
                AssignmentTitle = s.Assignment.Title,
                StudentId = s.StudentId,
                StudentName = s.Student.username,
                s.FileUrl,
                s.SubmittedOn,
                s.Marks
            })
            .ToListAsync();
    }
    public async Task<Submission> CreateSubmissionAsync(int assignmentId, int studentId, string fileUrl)
{
    var submission = new Submission
    {
        AssignmentId = assignmentId,
        StudentId = studentId,
        FileUrl = fileUrl,
        SubmittedOn = DateTime.Now
    };

    _context.Submissions.Add(submission);
    await _context.SaveChangesAsync();
    return submission;
}
}

using api.models.Assignment;
using Connect.Controllers.Authentication.Dto;
using System.Security.Claims;

namespace Connect.Interfaces
{
    public interface ISubmissionService
    {
       
        Task<IEnumerable<object>> GetSubmissionsByAssignmentIdAsync(int assignmentId);
        Task<bool> DeleteSubmissionAsync(int submissionId);
        Task<IEnumerable<object>> GetAllSubmissionsAsync();
        Task<Submission> CreateSubmissionAsync(int assignmentId, int studentId, string fileUrl);
    }
}

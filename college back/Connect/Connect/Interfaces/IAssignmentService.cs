using api.models.Assignment;
using Connect.Controllers.Authentication.Dto;
using System.Security.Claims;

namespace Connect.Interfaces
{
    public interface IAssignmentService
    {
        Task<Assignments> CreateAssignmentAsync(CreateAssignmentDto dto, ClaimsPrincipal user);
        Task<IEnumerable<object>> GetAllAssignmentsAsync();
        Task<object?> GetAssignmentByIdAsync(int id);
        Task<bool> DeleteAssignmentAsync(int id);
    }
}


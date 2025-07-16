using api.Data;
using api.models.Assignment;

using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ConnectDbContext _context;

        public AssignmentService(ConnectDbContext context)
        {
            _context = context;
        }

        public async Task<Assignments> CreateAssignmentAsync(CreateAssignmentDto dto, ClaimsPrincipal user)
        {
            int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));  
            string userName = user.FindFirstValue(ClaimTypes.Name);
            var assignment = new Assignments
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                CreatedById = userId
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return assignment;
        }

        public async Task<IEnumerable<object>> GetAllAssignmentsAsync()
        {
            return await _context.Assignments
                .Include(a => a.CreatedBy)
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    a.Description,
                    a.DueDate,
                    CreatedBy = a.CreatedBy.username
                }).ToListAsync();
        }

        public async Task<object?> GetAssignmentByIdAsync(int id)
        {
            var assignment = await _context.Assignments
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null) return null;

            return new
            {
                assignment.Id,
                assignment.Title,
                assignment.Description,
                assignment.DueDate,
                CreatedBy = assignment.CreatedBy.username
            };
        }

        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null) return false;

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
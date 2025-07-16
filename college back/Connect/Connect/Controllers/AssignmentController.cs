
using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string userName = User.FindFirstValue(ClaimTypes.Name);

            // Delegate creation to your service
            var assignment = await _assignmentService.CreateAssignmentAsync(dto, User);
            return Ok(new
            {
                message = "Assignment created successfully",
                assignmentId = assignment.Id,
                createdBy = new { userId, userName }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssignments()
        {
            var result = await _assignmentService.GetAllAssignmentsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
            if (assignment == null)
                return NotFound(new { message = "Assignment not found" });
            return Ok(assignment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var deleted = await _assignmentService.DeleteAssignmentAsync(id);
            if (!deleted)
                return NotFound(new { message = "Assignment not found" });
            return Ok(new { message = "Assignment deleted successfully" });
        }
    }
}
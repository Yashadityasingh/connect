using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SubmissionController : ControllerBase
{
    private readonly ISubmissionService _submissionService;

    public SubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpPost("{assignmentId}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> SubmitAssignment(int assignmentId, [FromBody] CreateSubmission dto)
    {
        int studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var submission = await _submissionService.CreateSubmissionAsync(assignmentId, studentId, dto.FileUrl);

        return Ok(new { message = "Submission successful", submission.Id });
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var submissions = await _submissionService.GetAllSubmissionsAsync();
        return Ok(submissions);
    }

    [HttpGet("{assignmentId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetByAssignment(int assignmentId)
    {
        var submissions = await _submissionService.GetSubmissionsByAssignmentIdAsync(assignmentId);
        return Ok(submissions);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _submissionService.DeleteSubmissionAsync(id);
        if (!deleted)
            return NotFound(new { message = "Submission not found" });

        return Ok(new { message = "Submission deleted" });
    }
}

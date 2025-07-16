using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupMembersController : ControllerBase
    {
        private readonly IGroupMemberService _service;
        public GroupMembersController(IGroupMemberService service) => _service = service;

        // POST: api/GroupMembers
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddMember([FromBody] AddGroupMemberByNameDto dto)
        {
            try
            {
                bool added = await _service.AddMemberByNameAsync(dto, User);
                if (!added) return BadRequest("User is already a member of the group.");
                return Ok(new { message = "Member added successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // DELETE: api/GroupMembers
        [HttpDelete]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> RemoveMember([FromBody] AddGroupMemberByNameDto dto)
        {
            try
            {
                bool removed = await _service.RemoveMemberByNameAsync(dto.GroupName, dto.Username, User);
                if (!removed) return NotFound("Membership not found.");
                return Ok(new { message = "Member removed successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // GET: api/GroupMembers/{groupName}
        [HttpGet("{groupName}")]
        public async Task<IActionResult> GetMembers(string groupName)
        {
            try
            {
                var members = await _service.GetMembersByGroupNameAsync(groupName);
                return Ok(members);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}

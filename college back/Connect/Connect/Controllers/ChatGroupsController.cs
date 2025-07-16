using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatGroupsController : ControllerBase
    {
        private readonly IChatGroupService _service;
        public ChatGroupsController(IChatGroupService service) => _service = service;

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] CreateChatGroupDto dto)
        {
            var result = await _service.CreateGroupAsync(dto, User);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllGroupsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var g = await _service.GetGroupByIdAsync(id);
            if (g == null) return NotFound();
            return Ok(g);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.DeleteGroupAsync(id)) return NotFound();
            return Ok(new { message = "Group deleted" });
        }
    }
}

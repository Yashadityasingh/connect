using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageService _service;

        public ChatMessagesController(IChatMessageService service)
            => _service = service;

        // JSON-only text endpoint
        [HttpPost("text")]
        [Consumes("application/json")]
        public async Task<IActionResult> SendText([FromBody] CreateChatMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var msg = await _service.SendMessageAsync(dto, User);
            return Ok(msg);
        }

        // multipart/form-data endpoint for image uploads
        [HttpPost]
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> SendForm([FromForm] CreateChatMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var msg = await _service.SendMessageAsync(dto, User);
            return Ok(msg);
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetByGroup(int groupId)
        {
            var list = await _service.GetMessagesByGroupAsync(groupId);
            return Ok(list);
        }
    }
}

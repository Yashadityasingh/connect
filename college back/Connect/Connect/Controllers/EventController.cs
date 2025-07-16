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
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var createdEvent = await _eventService.CreateEventAsync(dto, User);
            return Ok(createdEvent);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var e = await _eventService.GetEventByIdAsync(id);
            if (e == null) return NotFound(new { message = "Event not found" });
            return Ok(e);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var deleted = await _eventService.DeleteEventAsync(id);
            if (!deleted) return NotFound(new { message = "Event not found" });
            return Ok(new { message = "Event deleted successfully" });
        }
    }
}

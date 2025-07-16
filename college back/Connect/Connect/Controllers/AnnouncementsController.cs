using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Connect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _svc;
        public AnnouncementController(IAnnouncementService svc) => _svc = svc;

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto dto)
        {
            var result = await _svc.CreateAsync(dto, User);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_svc.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _svc.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _svc.DeleteAsync(id)) return NotFound();
            return Ok("Announcement deleted successfully.");
        }
    }
}

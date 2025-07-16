using api.Data;
using api.models.Events;
using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Connect.Services
{
    public class EventService : IEventService
    {
        private readonly ConnectDbContext _context;

        public EventService(ConnectDbContext context)
        {
            _context = context;
        }

        public async Task<EventDto> CreateEventAsync(CreateEventDto dto, ClaimsPrincipal user)
        {
            int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                EventDate = dto.EventDate,
                CreatedById = userId,
                TargetAudienceType = dto.TargetAudienceType,
                SpecificGroupId = dto.SpecificGroupId
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return new EventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                EventDate = ev.EventDate,
                CreatedById = ev.CreatedById,
                CreatedByUser = (await _context.Users.FindAsync(userId))?.username ?? "Unknown",
                TargetAudienceType = ev.TargetAudienceType,
                SpecificGroupId = ev.SpecificGroupId
            };
        }


        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.CreatedBy)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    CreatedById = e.CreatedById,
                    CreatedByUser = e.CreatedBy.username,
                    TargetAudienceType = e.TargetAudienceType,
                    SpecificGroupId = e.SpecificGroupId
                }).ToListAsync();
        }

        public async Task<EventDto?> GetEventByIdAsync(int id)
        {
            var e = await _context.Events.Include(x => x.CreatedBy).FirstOrDefaultAsync(x => x.Id == id);
            if (e == null) return null;

            return new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                EventDate = e.EventDate,
                CreatedById = e.CreatedById,
                CreatedByUser = e.CreatedBy.username,
                TargetAudienceType = e.TargetAudienceType,
                SpecificGroupId = e.SpecificGroupId
            };
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var e = await _context.Events.FindAsync(id);
            if (e == null) return false;

            _context.Events.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

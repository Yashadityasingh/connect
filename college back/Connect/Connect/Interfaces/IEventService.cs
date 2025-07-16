using Connect.Controllers.Authentication.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Connect.Interfaces
{
    public interface IEventService
    {
        Task<EventDto> CreateEventAsync(CreateEventDto dto, ClaimsPrincipal user);
        Task<IEnumerable<EventDto>> GetAllEventsAsync();
        Task<EventDto?> GetEventByIdAsync(int id);
        Task<bool> DeleteEventAsync(int id);
    }
}
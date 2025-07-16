using Connect.Controllers.Authentication.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Connect.Interfaces
{
    public interface IAnnouncementService
    {
        Task<AnnouncementsDto> CreateAsync(CreateAnnouncementDto dto, ClaimsPrincipal user);
        List<AnnouncementsDto> GetAll();
        AnnouncementsDto GetById(int id);
        Task<bool> DeleteAsync(int id);
    }
}

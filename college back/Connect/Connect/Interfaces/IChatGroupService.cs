using Connect.Controllers.Authentication.Dto;
using System.Security.Claims;

namespace Connect.Interfaces
{
    public interface IChatGroupService
    {
        Task<ChatGroupDto> CreateGroupAsync(CreateChatGroupDto dto, ClaimsPrincipal user);
        Task<IEnumerable<ChatGroupDto>> GetAllGroupsAsync();
        Task<ChatGroupDto?> GetGroupByIdAsync(int id);
        Task<bool> DeleteGroupAsync(int id);
    }
}

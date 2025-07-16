using Connect.Controllers.Authentication.Dto;
using System.Security.Claims;

namespace Connect.Interfaces
{
    public interface IChatMessageService
    {
        Task<ChatMessageDto> SendMessageAsync(CreateChatMessageDto dto, ClaimsPrincipal user);
        Task<IEnumerable<ChatMessageDto>> GetMessagesByGroupAsync(int groupId);
    }
}



using api.Data;
using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;             // for IChatMessageService
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class ChatMessageService : IChatMessageService
{
    private readonly ConnectDbContext _context;
    private readonly IFileStorageService _files;

    public ChatMessageService(
        ConnectDbContext context,
        IFileStorageService files)
    {
        _context = context;
        _files = files;
    }

    public async Task<ChatMessageDto> SendMessageAsync(CreateChatMessageDto dto, ClaimsPrincipal user)
    {
        // 1) Determine sender
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        var username = user.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrWhiteSpace(dto.Text) && dto.Image == null)
        {
            throw new ArgumentException("Either text or image must be provided.");
        }
        // 2) If an image was provided, store it
        string imageUrl = null;
        if (dto.Image != null && dto.Image.Length > 0)
        {
            imageUrl = await _files.SaveFileAsync(dto.Image);
        }

        // 3) Create entity
        var msg = new api.models.Chat.ChatMessage
        {
            GroupId = dto.GroupId,
            SenderId = userId,
            Text = dto.Text,
            SentOn = DateTime.UtcNow,
            ImageUrl = imageUrl    
        };

        _context.ChatMessages.Add(msg);
        await _context.SaveChangesAsync();

        // 4) Return DTO
        return new ChatMessageDto
        {
            Id = msg.Id,
            GroupId = msg.GroupId,
            SenderId = userId,
            SenderUser = username,
            Text = msg.Text,
            SentOn = msg.SentOn,
            ImageUrl = imageUrl
        };
    }

    public async Task<IEnumerable<ChatMessageDto>> GetMessagesByGroupAsync(int groupId)
    {
        return await _context.ChatMessages
            .Where(m => m.GroupId == groupId)
            .Include(m => m.Sender)
            .OrderBy(m => m.SentOn)
            .Select(m => new ChatMessageDto
            {
                Id = m.Id,
                GroupId = m.GroupId,
                SenderId = m.SenderId,
                SenderUser = m.Sender.username,
                Text = m.Text,
                SentOn = m.SentOn,
                ImageUrl = m.ImageUrl
            })
            .ToListAsync();
    }
}

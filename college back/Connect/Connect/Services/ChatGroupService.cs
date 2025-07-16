using api.Data;
using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Connect.Services.Implementations
{
    public class ChatGroupService : IChatGroupService
    {
        private readonly ConnectDbContext _context;
        public ChatGroupService(ConnectDbContext context) => _context = context;

        public async Task<ChatGroupDto> CreateGroupAsync(CreateChatGroupDto dto, ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var username = user.FindFirstValue(ClaimTypes.Name);

            var group = new api.models.Chat.ChatGroup
            {
                Name = dto.Name,
                Type = dto.Type,
                CreatedById = userId
            };

            _context.ChatGroups.Add(group);
            await _context.SaveChangesAsync();

            return new ChatGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Type = group.Type,
                CreatedById = userId,
                CreatedByUser = username
            };
        }

        public async Task<IEnumerable<ChatGroupDto>> GetAllGroupsAsync()
        {
            return await _context.ChatGroups
                .Include(g => g.CreatedBy)
                .Select(g => new ChatGroupDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Type = g.Type,
                    CreatedById = g.CreatedById,
                    CreatedByUser = g.CreatedBy.username
                }).ToListAsync();
        }

        public async Task<ChatGroupDto?> GetGroupByIdAsync(int id)
        {
            var g = await _context.ChatGroups
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (g == null) return null;
            return new ChatGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Type = g.Type,
                CreatedById = g.CreatedById,
                CreatedByUser = g.CreatedBy.username
            };
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            var g = await _context.ChatGroups.FindAsync(id);
            if (g == null) return false;
            _context.ChatGroups.Remove(g);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

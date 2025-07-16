using api.Data;
using api.models.Chat;
using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Services.Implementations
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly ConnectDbContext _context;
        public GroupMemberService(ConnectDbContext context) => _context = context;

        public async Task<bool> AddMemberByNameAsync(AddGroupMemberByNameDto dto, ClaimsPrincipal currentUser)
        {
            // 1. Find group
            var group = await _context.ChatGroups
                                      .FirstOrDefaultAsync(g => g.Name == dto.GroupName);
            if (group == null)
                throw new ArgumentException($"Chat group '{dto.GroupName}' not found.");

            // 2. Find user
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.username == dto.Username);
            if (user == null)
                throw new ArgumentException($"User '{dto.Username}' not found.");

            // 3. Authorization: only Admin or group creator
            var currentUserId = int.Parse(currentUser.FindFirstValue(ClaimTypes.NameIdentifier));
            bool isCreator = group.CreatedById == currentUserId;
            bool isAdmin = currentUser.IsInRole("Admin");
            if (!isCreator && !isAdmin)
                throw new UnauthorizedAccessException("Only the group creator or Admin can add members.");

            // 4. Prevent duplicates
            bool already = await _context.GroupMembers
                                         .AnyAsync(gm => gm.GroupId == group.Id && gm.UserId == user.Id);
            if (already) return false;

            // 5. Add membership
            _context.GroupMembers.Add(new GroupMember
            {
                GroupId = group.Id,
                UserId = user.Id
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMemberByNameAsync(string groupName, string username, ClaimsPrincipal currentUser)
        {
            var group = await _context.ChatGroups
                                      .FirstOrDefaultAsync(g => g.Name == groupName);
            if (group == null)
                throw new ArgumentException($"Chat group '{groupName}' not found.");

            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.username == username);
            if (user == null)
                throw new ArgumentException($"User '{username}' not found.");

            var currentUserId = int.Parse(currentUser.FindFirstValue(ClaimTypes.NameIdentifier));
            bool isCreator = group.CreatedById == currentUserId;
            bool isAdmin = currentUser.IsInRole("Admin");
            if (!isCreator && !isAdmin)
                throw new UnauthorizedAccessException("Only the group creator or Admin can remove members.");

            var membership = await _context.GroupMembers
                                           .FirstOrDefaultAsync(gm => gm.GroupId == group.Id && gm.UserId == user.Id);
            if (membership == null) return false;

            _context.GroupMembers.Remove(membership);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<string>> GetMembersByGroupNameAsync(string groupName)
        {
            var group = await _context.ChatGroups
                                      .FirstOrDefaultAsync(g => g.Name == groupName);
            if (group == null)
                throw new ArgumentException($"Chat group '{groupName}' not found.");

            return await _context.GroupMembers
                                 .Where(gm => gm.GroupId == group.Id)
                                 .Include(gm => gm.User)
                                 .Select(gm => gm.User.username)
                                 .ToListAsync();
        }
    }
}

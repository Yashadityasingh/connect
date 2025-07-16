using Connect.Controllers.Authentication.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Connect.Interfaces
{
    public interface IGroupMemberService
    {
        /// <summary>Adds a user (by username) to a chat group (by name).</summary>
        Task<bool> AddMemberByNameAsync(AddGroupMemberByNameDto dto, ClaimsPrincipal currentUser);

        /// <summary>Removes a user (by username) from a chat group (by name).</summary>
        Task<bool> RemoveMemberByNameAsync(string groupName, string username, ClaimsPrincipal currentUser);

        /// <summary>Lists all usernames in the given chat group.</summary>
        Task<IEnumerable<string>> GetMembersByGroupNameAsync(string groupName);
    }
}

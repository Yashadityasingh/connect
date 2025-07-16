using api.Data;
using api.models.Announcements;
using api.models.User;
using Connect.Controllers.Authentication.Dto;
using Connect.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Connect.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ConnectDbContext _db;
        public AnnouncementService(ConnectDbContext db) => _db = db;

        public async Task<AnnouncementsDto> CreateAsync(CreateAnnouncementDto dto, ClaimsPrincipal user)
        {
            // 1) Extract claims
            int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            string userName = user.FindFirstValue(ClaimTypes.Name);

            // 2) Build & save entity
            var a = new Announcement
            {
                Title = dto.Title,
                Message = dto.Message,
                Category = dto.Category,
                PostedById = userId,
                PostedByName = userName,
                PostedOn = DateTime.UtcNow
            };
            _db.Announcements.Add(a);
            await _db.SaveChangesAsync();

            // 3) Return DTO
            return new AnnouncementsDto
            {
                Id = a.Id,
                Title = a.Title,
                Message = a.Message,
                PostedById = a.PostedById,
                PostedByName = a.PostedByName,
                Category = a.Category,
                PostedOn = a.PostedOn
            };
        }

        public List<AnnouncementsDto> GetAll()
        {
            return _db.Announcements
                .OrderByDescending(a => a.PostedOn)
                .Select(a => new AnnouncementsDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Message = a.Message,
                    PostedById = a.PostedById,
                    PostedByName = a.PostedByName,
                    Category = a.Category,
                    PostedOn = a.PostedOn
                })
                .ToList();
        }

        public AnnouncementsDto GetById(int id)
        {
            var a = _db.Announcements.Find(id);
            if (a == null) return null;
            return new AnnouncementsDto
            {
                Id = a.Id,
                Title = a.Title,
                Message = a.Message,
                PostedById = a.PostedById,
                PostedByName = a.PostedByName,
                Category = a.Category,
                PostedOn = a.PostedOn
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var a = await _db.Announcements.FindAsync(id);
            if (a == null) return false;
            _db.Announcements.Remove(a);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

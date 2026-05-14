using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;

namespace TrainTicket.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly TrainTicketDbContext _db;

        public NotificationService(TrainTicketDbContext db) => _db = db;

        public async Task<List<NotificationDto>> GetUnreadAsync(int userId) =>
            await _db.Notifications
                .Where(n => n.UserID == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .Select(n => new NotificationDto
                {
                    NotiID    = n.NotiID,
                    Title     = n.Title,
                    Body      = n.Body,
                    Type      = n.Type,
                    IsRead    = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    RelatedID = n.RelatedID
                }).ToListAsync();

        public async Task<int> GetUnreadCountAsync(int userId) =>
            await _db.Notifications.CountAsync(n => n.UserID == userId && !n.IsRead);

        public async Task MarkReadAsync(int notiId)
        {
            var n = await _db.Notifications.FindAsync(notiId);
            if (n != null) { n.IsRead = true; await _db.SaveChangesAsync(); }
        }

        public async Task MarkAllReadAsync(int userId)
        {
            await _db.Notifications
                .Where(n => n.UserID == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
        }
    }
}
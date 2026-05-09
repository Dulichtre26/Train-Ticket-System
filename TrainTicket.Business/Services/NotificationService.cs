using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace TrainTicket.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly TrainTicketDbContext _context;

        public NotificationService(TrainTicketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications.Select(n => new NotificationDto
            {
                NotiID = n.NotiID,
                UserID = n.UserID,
                Title = n.Title,
                Body = n.Body,
                Type = n.Type,
                IsRead = n.IsRead,
                RelatedID = n.RelatedID,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task<bool> MarkAsReadAsync(int notiId, int userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotiID == notiId && n.UserID == userId);

            if (notification == null) return false;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendNotificationAsync(int userId, string title, string body, string type = "Info", int? relatedId = null)
        {
            var notification = new Notification
            {
                UserID = userId,
                Title = title,
                Body = body,
                Type = type,
                RelatedID = relatedId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
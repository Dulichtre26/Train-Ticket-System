using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notiId, int userId);
        Task<bool> SendNotificationAsync(int userId, string title, string body, string type = "Info", int? relatedId = null);
    }
}
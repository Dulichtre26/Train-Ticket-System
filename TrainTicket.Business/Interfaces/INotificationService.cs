using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUnreadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkReadAsync(int notiId);
        Task MarkAllReadAsync(int userId);
    }
}
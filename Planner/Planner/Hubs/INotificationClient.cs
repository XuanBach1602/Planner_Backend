using Planner.Model;

namespace Planner.Hubs
{
    public interface INotificationClient
    {
        Task SendNotificationToUser(string userId, NotificationOutput notificationOutput);
    }
}

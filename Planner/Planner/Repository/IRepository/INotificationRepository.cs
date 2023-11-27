using Planner.Model;
using System.Linq.Expressions;

namespace Planner.Repository.IRepository
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationOutput>> GetAllAsync(Expression<Func<Notification, bool>> filter);
        Task<NotificationOutput?> GetAsync(Expression<Func<Notification, bool>> filter);
        Task<Notification?> AddAsync(NotificationInput notificationInput);
        Task UpdateStatus(int id, string status);
        Task UpdateIsSeen(int id);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Linq.Expressions;

namespace Planner.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly PlannerDbContext _context;
        public NotificationRepository(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> AddAsync(NotificationInput notificationInput)
        {
            var notification = ConvertToNotification(notificationInput);
            var notificationList = await GetAsync(x => x.PlanId == notification.PlanId
                && x.ReceivedUserId == notification.ReceivedUserId && x.Title == "Invitation" && x.Status == "Not responsed");
            if (notificationList == null)
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                return notification;
            }

            return null;
        }


        public async Task<IEnumerable<NotificationOutput>> GetAllAsync(Expression<Func<Notification, bool>> filter)
        {
            IQueryable<Notification> query = _context.Notifications
                .Include(x => x.Plan)
                .Include(x => x.SendedUser)
                .Include(x => x.ReceivedUser)
                .Where(filter);
            var notificationList = await query.ToListAsync();
            var notificationOutputList = notificationList.Select(ConvertToNotificationOutput);

            return notificationOutputList;
        }

        public async Task UpdateStatus(int id, string status)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (notification != null)
            {
                notification.Status = status;
            }

        }

        public async Task UpdateIsSeen(int id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (notification != null)
            {
                notification.IsSeen = true;
            }
        }

        public async Task<NotificationOutput?> GetAsync(Expression<Func<Notification, bool>> filter)
        {
            var notification = await _context.Notifications
                .Include(x => x.Plan)
                .Include(x => x.SendedUser)
                .Include(x => x.ReceivedUser)
                .FirstOrDefaultAsync(filter);
            if (notification != null)
            {
                var notificationOutput = ConvertToNotificationOutput(notification);
                return notificationOutput;
            }

            return null;


        }

        public NotificationOutput ConvertToNotificationOutput(Notification notification)
        {
            return new NotificationOutput
            {
                Id = notification.Id,
                Status = notification.Status,
                Title = notification.Title,
                IsSeen = notification.IsSeen,
                PlanId = notification.PlanId,
                PlanName = notification.Plan.Name,
                ReceivedUserId = notification.ReceivedUserId,
                ReceivedUserName = notification.ReceivedUser.Name,
                SendedUserId = notification.SendedUserId,
                SendedUserName = notification.SendedUser.Name,
                CreatedTime = notification.CreatedTime,
                ResponseTime = notification.ResponseTime,
                ImgUrl = notification.SendedUser.ImgUrl,
                WorkTaskId = notification.WorkTaskId,

            };
        }

        [NonAction]
        public Notification ConvertToNotification(NotificationInput notificationInput)
        {
            return new Notification
            {
                Title = notificationInput.Title,
                IsSeen = notificationInput.IsSeen,
                ReceivedUserId = notificationInput.ReceivedUserId,
                SendedUserId = notificationInput.SendedUserId,
                Status = "Not responsed",
                CreatedTime = DateTime.Now,
                PlanId = notificationInput.PlanId,
                WorkTaskId = notificationInput.WorkTaskId,
            };
        }
    }
}

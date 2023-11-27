using Microsoft.AspNetCore.SignalR;
using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly Dictionary<string, string> UserConnections = new Dictionary<string, string>();
        private readonly IUnitOfWork _unitOfWork;
        public NotificationHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserId(string userId)
        {
            var connectionId = Context.ConnectionId;

            lock (UserConnections)
            {
                if (!UserConnections.ContainsKey(userId))
                {
                    UserConnections.Add(userId, connectionId);
                }
                else
                {
                    UserConnections[userId] = connectionId;
                }
            }

            await Clients.Caller.SendAsync("UserIdAdded", userId);
        }


        public async Task SendNotificationToUser(string targetUserId, NotificationInput notificationInput)
        {
            var notification = await _unitOfWork.Notification.AddAsync(notificationInput);


            if (notification != null)
            {
                if (UserConnections.TryGetValue(targetUserId, out string targetConnectionId))
                {
                    var notificationOutput = await _unitOfWork.Notification.GetAsync(x => x.Id == notification.Id);
                    await Clients.Client(targetConnectionId).SendAsync("ReceiveMessage", notificationOutput);
                }
                else
                {
                    // Xử lý khi không tìm thấy kết nối của người dùng đích
                }
            }

        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var disconnectedConnectionId = Context.ConnectionId;

            lock (UserConnections)
            {
                var userToRemove = UserConnections.FirstOrDefault(x => x.Value == disconnectedConnectionId);
                if (!string.IsNullOrEmpty(userToRemove.Key))
                {
                    UserConnections.Remove(userToRemove.Key);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }

}

namespace Planner.Model
{
    public class NotificationInput
    {
        public required string Title { get; set; }
        public bool IsSeen { get; set; } = false;
        public required string ReceivedUserId { get; set; }
        public required string SendedUserId { get; set; }
        public required int PlanId { get; set; }
        public int? WorkTaskId { get; set; }
    }
}

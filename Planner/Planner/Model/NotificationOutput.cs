namespace Planner.Model
{
    public class NotificationOutput
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsSeen { get; set; }
        public int PlanId { get; set; }
        public required string PlanName { get; set; }
        public required string ReceivedUserId { get; set; }
        public required string ReceivedUserName { get; set; }
        public required string SendedUserId { get; set; }
        public required string SendedUserName { get; set; }
        public required string ImgUrl { get; set; }
        public DateTime CreatedTime { get; set; }
        public required string Status { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int? WorkTaskId { get; internal set; }
        public string? WorkTaskName { get; set; }
    }
}

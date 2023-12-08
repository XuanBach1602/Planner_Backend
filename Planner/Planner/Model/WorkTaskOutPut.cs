namespace Planner.Model
{
    public class WorkTaskOutput
    {
        public int Id { get; set; }
        public int? OriginId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
        public required string Priority { get; set; }
        public required string StartDate { get; set; }
        public required string DueDate { get; set; }
        public required bool IsPrivate { get; set; }
        public int CategoryId { get; set; }
        public string? CompletedUserId { get; set; }
        public int PlanId { get; set; }
        public required string CreatedUserId { get; set; }
        public string? AssignedUserId { get; set; }
        public string ModifiedDate { get; set; }
        public string? CompletedTime { get; set; }
        public bool IsApproved { get; set; }
        public List<UploadFile> Files { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsUpdateTask { get; set; }
        public string? OriginName { get; set; }
        public string Frequency { get; set; } = "Daily";
        public string StartTime { get; set; } = "08:00";
        public string EndTime { get; set; } = "10:00";
    }
}

﻿namespace Planner.Model
{
    public class WorkTaskOutput
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
        public required string Priority { get; set; }
        public required string StartDate { get; set; }
        public required string DueDate { get; set; }
        public int CategoryId { get; set; }
        public int PlanId { get; set; }
        public required string CreatedUserId { get; set; }
        public string? AssignedUserId { get; set; }
        public string ModifiedDate { get; set; }
        public List<UploadFile> Files { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}

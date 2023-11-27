using System.ComponentModel.DataAnnotations;

namespace Planner.Model
{
    public class TemporaryWorkTask
    {
        [Key]
        public int Id { get; set; } = 0;
        public int WorkTaskId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        public required string Priority { get; set; }
        public bool IsPrivate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public string? AssignedUserID { get; set; }
        public string? CompletedUserID { get; set; }
        public bool IsApproved { get; set; }
        //public ICollection<UploadFile>? Files { get; set; }
    }
}

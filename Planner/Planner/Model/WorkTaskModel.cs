using System.ComponentModel.DataAnnotations;

namespace Planner.Model
{
    public class WorkTaskModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int PlanID { get; set; }
        [Required]
        public string CreatedUserID { get; set; } = string.Empty;
        public string? AssignedUserID { get; set; }
        public List<IFormFile>? AttachedFiles { get; set; }

    }
}

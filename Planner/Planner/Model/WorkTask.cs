
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Model
{
    public class WorkTask
    {
        [Key]
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
        [ForeignKey("Plans")]
        public int PlanID { get; set; }
        [ForeignKey(nameof(PlanID))]
        public Plan? Plan { get; set; }

        [ForeignKey("Users")]
        public string CreatedUserID { get; set; }
        [ForeignKey(nameof(CreatedUserID))]
        public User? CreatedUser { get; set; }

        [ForeignKey("Users")]
        public string? AssignedUserID { get; set; }
        [ForeignKey(nameof(AssignedUserID))]
        public User? AssignedUser { get; set; }


    }
}

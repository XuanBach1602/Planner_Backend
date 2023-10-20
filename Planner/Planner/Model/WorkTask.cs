
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Plan? Plan { get; set; }

        [ForeignKey("Users")]
        public string CreatedUserID { get; set; }
        [ForeignKey(nameof(CreatedUserID))]
        [JsonIgnore]
        public User? CreatedUser { get; set; }

        [ForeignKey("Users")]
        public string? AssignedUserID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(AssignedUserID))]
        public User? AssignedUser { get; set; }


    }
}

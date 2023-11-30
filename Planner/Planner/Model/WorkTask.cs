
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Planner.Model
{
    public class WorkTask
    {
        [Key]
        public int Id { get; set; }
        public int? OriginId { get; set; }
        public WorkTask? Origin { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public string Priority { get; set; } = "Medium";
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
        public int PlanId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? CompletedTime { get; set; }

        //public string? Attachment { get; set; }

        [Required]
        [ForeignKey("Categories")]
        public int CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        [JsonIgnore]
        public Category? Category { get; set; }

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
        public string? CompletedUserId { get; set; }
        [ForeignKey("CompletedUserId")]
        [JsonIgnore]
        public User? CompletedUser { get; set; }
        public bool IsUpdateTask { get; set; }

        public ICollection<UploadFile> Files { get; set; }


    }
}

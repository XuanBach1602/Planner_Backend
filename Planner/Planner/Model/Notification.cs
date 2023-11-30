using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Planner.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        //public string Content { get; set; } = string.Empty;
        public bool IsSeen { get; set; }
        public int PlanId { get; set; }
        [ForeignKey("PlanId")]
        [JsonIgnore]
        public Plan? Plan { get; set; }
        public int? WorkTaskId { get; set; }
        public required string ReceivedUserId { get; set; }
        [ForeignKey("ReceivedUserId")]
        [JsonIgnore]
        public User? ReceivedUser { get; set; }
        public required string SendedUserId { get; set; }
        [ForeignKey("SendedUserId")]
        [JsonIgnore]
        public User? SendedUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public required string Status { get; set; }
        public DateTime? ResponseTime { get; set; }


    }
}

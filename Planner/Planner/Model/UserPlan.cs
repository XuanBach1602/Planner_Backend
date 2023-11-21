using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Model
{
    public class UserPlan
    {
        public int Id { get; set; }
        [ForeignKey("Users")]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("Plans")]
        public int PlanId { get; set; }
        public string Role { get; set; } = string.Empty;
        public User? User { get; set; }
        public Plan? Plan { get; set; }
    }
}

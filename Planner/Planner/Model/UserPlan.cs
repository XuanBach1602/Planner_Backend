using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Model
{
    public class UserPlan
    {
        public int Id { get; set; }
        [ForeignKey("Users")]
        public string UserId { get; set; }
        [ForeignKey("Plans")]
        public int PlanId { get; set; }
        public User? User { get; set; }
        public Plan? Plan { get; set; }
    }
}

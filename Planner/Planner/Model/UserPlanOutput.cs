namespace Planner.Model
{
    public class UserPlanOutput
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int PlanId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

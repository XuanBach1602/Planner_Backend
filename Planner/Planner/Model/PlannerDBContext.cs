using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Planner.Model
{
    public class PlannerDbContext : IdentityDbContext<User>
    {
        public PlannerDbContext(DbContextOptions<PlannerDbContext> options) : base(options) { }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }
    }
}

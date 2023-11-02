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
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<WorkTask>()
                .Property(x => x.StartDate)
                .HasColumnType("date");
            builder.Entity<WorkTask>()
               .Property(x => x.DueDate)
               .HasColumnType("date");
            builder.Entity<User>()
                .Property(x => x.DateOfBirth)
                .HasColumnType("date");
        }
    }
}

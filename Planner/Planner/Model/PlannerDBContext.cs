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
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        //public DbSet<TemporaryWorkTask> TemporaryWorkTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<WorkTask>()
            //    .Property(x => x.StartDate)
            //    .HasColumnType("date");
            //builder.Entity<WorkTask>()
            //   .Property(x => x.DueDate)
            //   .HasColumnType("date");
            builder.Entity<User>()
                .Property(x => x.DateOfBirth)
                .HasColumnType("date");
            builder.Entity<WorkTask>()
                .HasMany<UploadFile>(f => f.Files)
                .WithOne(f => f.WorkTask)
                .HasForeignKey(f => f.WorkTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(x => x.SendedUser)
                .WithMany()
                .HasForeignKey(x => x.SendedUserId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .HasOne(x => x.ReceivedUser)
                .WithMany()
                .HasForeignKey(x => x.ReceivedUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

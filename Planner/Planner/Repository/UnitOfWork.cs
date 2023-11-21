using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlannerDbContext _db;
        public UnitOfWork(IPlanRepository plannerRepository, IWorkTaskRepository workTaskRepository, PlannerDbContext db,
            IUserRepository user, ICategoryRepository categoryRepository, IUploadFileRepository uploadFileRepository,
            IUserPlanRepository userPlan, INotificationRepository notificationRepository)
        {
            Plan = plannerRepository;
            WorkTask = workTaskRepository;
            _db = db;
            User = user;
            Category = categoryRepository;
            UploadFile = uploadFileRepository;
            UserPlan = userPlan;
            Notification = notificationRepository;
        }

        public IPlanRepository Plan { get; set; }

        public IWorkTaskRepository WorkTask { get; set; }
        public IUserRepository User { get; set; }
        public ICategoryRepository Category { get; set; }
        public IUploadFileRepository UploadFile { get; set; }
        public IUserPlanRepository UserPlan { get; set; }
        public INotificationRepository Notification { get; set; }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}

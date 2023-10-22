using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlannerDbContext _db;
        public UnitOfWork(IPlanRepository plannerRepository, IWorkTaskRepository workTaskRepository, PlannerDbContext db, IUserRepository user, ICategoryRepository categoryRepository)
        {
            Plan = plannerRepository;
            WorkTask = workTaskRepository;
            _db = db;
            User = user;
            Category = categoryRepository;
        }

        public IPlanRepository Plan { get; set; }

        public IWorkTaskRepository WorkTask { get; set; }
        public IUserRepository User { get; set; }
        public ICategoryRepository Category { get; set; }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}

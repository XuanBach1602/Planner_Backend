namespace Planner.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IPlanRepository Plan { get; }
        public IWorkTaskRepository WorkTask { get; }
        public IUserRepository User { get; }
        Task Save();
    }
}

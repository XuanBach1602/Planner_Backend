namespace Planner.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IPlanRepository Plan { get; }
        public IWorkTaskRepository WorkTask { get; }
        public IUserRepository User { get; }
        public ICategoryRepository Category { get; }
        public IUploadFileRepository UploadFile { get; }
        Task Save();
    }
}

using Planner.Model;

namespace Planner.Repository.IRepository
{
    public interface ITemporaryWorkTaskRepository
    {
        Task AddAsync(TemporaryWorkTask temporaryWorkTask);
        Task Remove(int id);
        Task<TemporaryWorkTask> FindById(int id);
    }
}

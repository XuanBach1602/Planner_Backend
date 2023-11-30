using Planner.Model;
using System.Linq.Expressions;
using static WorkTaskner.Repository.WorkTaskRepository;

namespace Planner.Repository.IRepository
{
    public interface IWorkTaskRepository
    {
        Task AddAsync(WorkTask WorkTask);
        Task<IEnumerable<WorkTaskOutput>> GetAllAsync(Expression<Func<WorkTask, bool>>? filter = null, string? includeProperties = null);
        Task<WorkTaskOutput> GetFirstOrDefaultAsync(Expression<Func<WorkTask, bool>> filter, string? includeProperties = null);
        void Update(WorkTask WorkTask);
        Task Remove(int id);
        Task UpdateStatus(int id, string status, string? userId);
        public Task<CountTasks> GetCountOfFilteredTask(int planId);
        public Task UpdateTaskById(int id);
    }
}

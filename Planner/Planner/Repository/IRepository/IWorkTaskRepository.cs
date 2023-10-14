using Planner.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Planner.Repository.IRepository
{
    public interface IWorkTaskRepository
    {
        Task AddAsync(WorkTask WorkTask);
        Task<IEnumerable<WorkTask>> GetAllAsync(Expression<Func<WorkTask, bool>>? filter = null, string? includeProperties = null);
        Task<WorkTask> GetFirstOrDefaultAsync(Expression<Func<WorkTask, bool>> filter, string? includeProperties = null);
        void Update(WorkTask WorkTask);
        void Remove(WorkTask WorkTask);
    }
}

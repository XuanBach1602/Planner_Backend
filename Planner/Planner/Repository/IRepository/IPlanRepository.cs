using Planner.Model;
using System.Linq.Expressions;

namespace Planner.Repository.IRepository
{
    public interface IPlanRepository
    {
        Task AddAsync(Plan plan);
        Task<IEnumerable<Plan>> GetAllAsync(Expression<Func<Plan, bool>>? filter = null, string? includeProperties = null);
        Task<Plan> GetFirstOrDefaultAsync(Expression<Func<Plan, bool>> filter, string? includeProperties = null);
        void Update(Plan plan);
        void Remove(Plan plan);
        Task<List<Plan>> GetPlansByUserID(string userId);
    }
}

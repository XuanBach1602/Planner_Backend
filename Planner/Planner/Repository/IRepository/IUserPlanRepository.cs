using Planner.Model;
using System.Linq.Expressions;

namespace Planner.Repository.IRepository
{
    public interface IUserPlanRepository
    {
        Task AddUserToPlan(int planId, string userId, string role);
        Task UpdateRole(int userPlanId, string role);
        Task RemoveUserFromPlan(int userPlanId);
        Task<IEnumerable<UserPlanOutput>> GetAllByFilter(Expression<Func<UserPlan, bool>> filter);
    }
}

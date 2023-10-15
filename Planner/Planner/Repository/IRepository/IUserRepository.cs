using Planner.Model;
using System.Linq.Expressions;

namespace Planner.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null);
        Task<User> GetFirstOrDefaultAsync(Expression<Func<User, bool>> filter);

        void Update(User user);
        Task<List<User>> GetUsersByPlanID(int planID);
    }
}

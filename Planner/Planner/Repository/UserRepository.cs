using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Linq;
using System.Linq.Expressions;

namespace Planner.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PlannerDbContext _context;
        public UserRepository(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = _context.Users;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<User> GetFirstOrDefaultAsync(Expression<Func<User, bool>> filter)
        {
            IQueryable<User> query = _context.Users;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public void Update(User user)
        {
            _context.Update(user);
        }

        public async Task<List<User>> GetUsersByPlanID(int planID)
        {
            var userPlan = _context.UserPlans
                .Include(x => x.User)
                .Where(x => x.PlanId == planID);
            var users = userPlan.Select(x => x.User);
            if(userPlan != null)
            {
                return await users.ToListAsync();
            }

            return new List<User>();
        }
    }
}

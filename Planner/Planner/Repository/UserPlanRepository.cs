using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Linq.Expressions;

namespace Planner.Repository
{
    public class UserPlanRepository : IUserPlanRepository
    {
        private readonly PlannerDbContext _context;
        public UserPlanRepository(PlannerDbContext context)
        {
            _context = context;
        }
        public async Task AddUserToPlan(int planId, string userId, string role)
        {
            var userPlan = new UserPlan
            {
                PlanId = planId,
                UserId = userId,
                Role = role
            };
            await _context.UserPlans.AddAsync(userPlan);
        }

        public async Task RemoveUserFromPlan(int userPlanId)
        {
            var userPlan = await _context.UserPlans.FindAsync(userPlanId);
            if (userPlan != null)
            {
                _context.UserPlans.Remove(userPlan);
            }
        }

        public async Task UpdateRole(int userPlanId, string role)
        {
            var userPlan = await _context.UserPlans.FindAsync(userPlanId);
            if (userPlan != null)
            {
                userPlan.Role = role;
            }
        }

        public async Task<IEnumerable<UserPlanOutput>> GetAllByFilter(Expression<Func<UserPlan, bool>> filter)
        {
            IQueryable<UserPlan> query = _context.UserPlans
                .Include(x => x.Plan)
                .Include(x => x.User);

            var userPlans = await query.Where(filter).ToListAsync();
            var filteredUserPlans = userPlans.Select(ConvertToUserPlanOutput);
            return filteredUserPlans; // Chú ý sử dụng ToListAsync nếu đây là một phương thức async
        }



        private UserPlanOutput ConvertToUserPlanOutput(UserPlan userPlan)
        {
            return new UserPlanOutput
            {
                Id = userPlan.Id,
                PlanId = userPlan.PlanId,
                UserId = userPlan.UserId,
                Role = userPlan.Role,
                UserName = userPlan.User.Name,
                PlanName = userPlan.Plan.Name,
                ImgUrl = userPlan.User.ImgUrl,
                Email = userPlan.User.Email
            };
        }
    }
}
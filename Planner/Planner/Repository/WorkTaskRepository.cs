using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Linq.Expressions;

namespace WorkTaskner.Repository
{
    public class WorkTaskRepository : IWorkTaskRepository
    {
        private readonly PlannerDbContext _context;
        public WorkTaskRepository(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WorkTask WorkTask)
        {
            await _context.WorkTasks.AddAsync(WorkTask);
        }

        public async Task<IEnumerable<WorkTask>> GetAllAsync(Expression<Func<WorkTask, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<WorkTask> query = _context.WorkTasks;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<WorkTask> GetFirstOrDefaultAsync(Expression<Func<WorkTask, bool>> filter, string? includeProperties = null)
        {
            IQueryable<WorkTask> query = _context.WorkTasks;
            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remove(WorkTask WorkTask)
        {
            _context.WorkTasks.Remove(WorkTask);
        }

        public void Update(WorkTask WorkTask)
        {
            _context.WorkTasks.Update(WorkTask);
        }
    }
}

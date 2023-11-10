using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Linq.Expressions;

namespace WorkTaskner.Repository
{
    public class WorkTaskRepository : IWorkTaskRepository
    {
        private readonly PlannerDbContext _context;
        private readonly IFileService _fileService;
        public WorkTaskRepository(PlannerDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
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
        public async Task<CountTasks> GetCountOfFilteredTask(int planId)
        {
            var tasks = await GetAllAsync(x => x.PlanId == planId);
            var notStartedTask = tasks.Where(x => x.Status == "Not started");
            var inProgressTask = tasks.Where(x => x.Status == "In progress" && x.DueDate >= DateTime.Now);
            var completedTask = tasks.Where(x => x.Status == "Completed");
            var lateTask = tasks.Where(x => x.Status == "In progress" && x.DueDate < DateTime.Now);
            return new CountTasks
            {
                NotStartedTasksCount = notStartedTask.Count(),
                InProgressTasksCount = inProgressTask.Count(),
                CompletedTasksCount = completedTask.Count(),
                LateTasksCount = lateTask.Count(),
            };
        }

        public void Remove(WorkTask WorkTask)
        {
            _context.WorkTasks.Remove(WorkTask);
        }

        public void Update(WorkTask WorkTask)
        {
            _context.WorkTasks.Update(WorkTask);
        }

        public class CountTasks
        {
            public int NotStartedTasksCount { get; set; }
            public int InProgressTasksCount { get; set; }
            public int CompletedTasksCount { get; set; }
            public int LateTasksCount { get; set; }
        }




    }

}


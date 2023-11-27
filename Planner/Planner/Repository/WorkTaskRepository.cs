using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using Planner.Services;
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

        public async Task<IEnumerable<WorkTaskOutput>> GetAllAsync(Expression<Func<WorkTask, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<WorkTask> query = _context.WorkTasks.Include(wt => wt.Category).Include(wt => wt.Files);
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
            var workTaskList = await query.ToListAsync();
            var formattedWorkTaskList = workTaskList.Select(ConvertToFormatted);
            return formattedWorkTaskList;
        }

        public async Task<WorkTaskOutput> GetFirstOrDefaultAsync(Expression<Func<WorkTask, bool>> filter, string? includeProperties = null)
        {
            IQueryable<WorkTask> query = _context.WorkTasks.Include(wt => wt.Category).Include(wt => wt.Files);
            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            var worktask = await query.FirstOrDefaultAsync();
            if (worktask != null) return ConvertToFormatted(worktask);
            else return null;
        }

        public async Task<CountTasks> GetCountOfFilteredTask(int planId)
        {
            var tasks = await GetAllAsync(x => x.PlanId == planId);
            var notStartedTask = tasks.Where(x => x.Status == "Not started");
            var inProgressTask = tasks.Where(x => x.Status == "In progress" && DateTime.Parse(x.DueDate) >= DateTime.Now);
            var completedTask = tasks.Where(x => x.Status == "Completed");
            var lateTask = tasks.Where(x => x.Status == "In progress" && DateTime.Parse(x.DueDate) < DateTime.Now);
            var tasksLeft = tasks.Where(x => x.Status != "Completed");
            return new CountTasks
            {
                NotStartedTasksCount = notStartedTask.Count(),
                InProgressTasksCount = inProgressTask.Count(),
                CompletedTasksCount = completedTask.Count(),
                LateTasksCount = lateTask.Count(),
                TaskLeft = tasksLeft.Count()
            };
        }

        public async Task Remove(int id)
        {
            var workTask = await _context.WorkTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (workTask != null) { _context.WorkTasks.Remove(workTask); }
            else throw new Exception("The work task is not found");
        }

        public void Update(WorkTask WorkTask)
        {
            _context.WorkTasks.Update(WorkTask);
        }

        public async Task UpdateStatus(int id, string status, string? userId)
        {
            var worktask = await _context.WorkTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (worktask != null)
            {
                if (status == "Completed")
                {
                    worktask.CompletedUserId = userId;
                    worktask.CompletedTime = DateTime.Now;
                }
                if (status != "Completed" && worktask.Status == "Completed")
                {
                    worktask.CompletedUserId = null;
                    worktask.CompletedTime = null;
                }
                worktask.Status = status;
            }
            else throw new Exception("Can not find the work task");
        }

        public class CountTasks
        {
            public int NotStartedTasksCount { get; set; }
            public int InProgressTasksCount { get; set; }
            public int CompletedTasksCount { get; set; }
            public int LateTasksCount { get; set; }
            public int TaskLeft { get; set; }
        }

        private WorkTaskOutput ConvertToFormatted(WorkTask workTask)
        {
            return new WorkTaskOutput
            {
                Id = workTask.Id,
                Name = workTask.Name,
                Description = workTask.Description,
                Status = workTask.Status,
                Priority = workTask.Priority,
                StartDate = workTask.StartDate.ToString("yyyy-MM-dd"),
                DueDate = workTask.DueDate.ToString("yyyy-MM-dd"),
                CategoryId = workTask.CategoryID,
                PlanId = workTask.PlanId,
                CreatedUserId = workTask.CreatedUserID,
                AssignedUserId = workTask.AssignedUserID,
                Files = workTask.Files.ToList(),
                ModifiedDate = workTask.ModifiedDate.ToString(),
                CategoryName = workTask.Category.Name,
                IsPrivate = workTask.IsPrivate,
                CompletedUserId = workTask.CompletedUserId,
                CompletedTime = workTask.CompletedTime?.ToString("yyyy-MM-dd"),
                IsApproved = workTask.IsApproved

            };
        }



    }

}


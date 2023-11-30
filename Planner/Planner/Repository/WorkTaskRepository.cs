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
            if (WorkTask.Status == "Completed")
            {
                WorkTask.CompletedUserId = WorkTask.CreatedUserID;
                WorkTask.CompletedTime = DateTime.Now;
            }
            await _context.WorkTasks.AddAsync(WorkTask);
        }

        public async Task<IEnumerable<WorkTaskOutput>> GetAllAsync(Expression<Func<WorkTask, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<WorkTask> query = _context.WorkTasks.Include(wt => wt.Category).Include(wt => wt.Files).Where(wt => wt.IsUpdateTask == false);
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
            IQueryable<WorkTask> query = _context.WorkTasks.Include(wt => wt.Category).Include(wt => wt.Files).Include(wt => wt.Origin);
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
            if (workTask != null)
            {

                var workTasks = await _context.WorkTasks.Where(x => x.OriginId == id).ToListAsync();
                if (workTasks != null)
                {
                    foreach (var worktask in workTasks)
                    {
                        var notifications = await _context.Notifications.Where(x => x.WorkTaskId == worktask.Id).ToListAsync();
                        _context.Notifications.RemoveRange(notifications);
                    }
                    _context.WorkTasks.RemoveRange(workTasks);
                }

                _context.WorkTasks.Remove(workTask);
            }
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

        public async Task UpdateTaskById(int id)
        {
            var updatedWorkTask = await _context.WorkTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (updatedWorkTask == null)
            {
                throw new Exception("Cannot find the worktask");
            }
            //if (updatedWorkTask.OriginId != null)
            //{
            //    var originId = updatedWorkTask.OriginId;
            //    //updatedWorkTask.IsUpdateTask = false;
            //    updatedWorkTask.OriginId = null;
            //    await _context.SaveChangesAsync();
            //    //await Remove((int)originId);
            //}
            if (updatedWorkTask.OriginId != null)
            {
                //updatedWorkTask.OriginId = null;

                var workTask = await _context.WorkTasks.FirstOrDefaultAsync(x => x.Id == updatedWorkTask.OriginId);
                workTask.Status = updatedWorkTask.Status;
                workTask.StartDate = updatedWorkTask.StartDate;
                workTask.DueDate = updatedWorkTask.DueDate;
                workTask.ModifiedDate = updatedWorkTask.ModifiedDate;
                workTask.IsPrivate = updatedWorkTask.IsPrivate;
                workTask.AssignedUserID = updatedWorkTask.AssignedUserID;
                workTask.CompletedUserId = updatedWorkTask.CompletedUserId;
                //workTask.IsUpdateTask = updatedWorkTask.IsUpdateTask;
                workTask.Files = updatedWorkTask.Files;
                workTask.CategoryID = updatedWorkTask.CategoryID;
                workTask.PlanId = updatedWorkTask.PlanId;
                workTask.CreatedUserID = updatedWorkTask.CreatedUserID;
                workTask.Name = updatedWorkTask.Name;
                workTask.Description = updatedWorkTask.Description;
                workTask.Priority = updatedWorkTask.Priority;
                workTask.CompletedTime = updatedWorkTask.CompletedTime;
                workTask.IsApproved = updatedWorkTask.IsApproved;
                workTask.ModifiedDate = updatedWorkTask.ModifiedDate;
                //await _context.SaveChangesAsync();
            }

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
                IsApproved = workTask.IsApproved,
                OriginId = workTask.OriginId,
                OriginName = workTask?.Origin?.Name


            };
        }



    }

}


using Planner.Model;

namespace Planner.Services
{
    public class FilterService : IFilterService
    {
        public IEnumerable<WorkTaskOutput> FilterWorkTask(IEnumerable<WorkTaskOutput> workTasks, string due, string priority, string progress)
        {
            if (!string.IsNullOrEmpty(due))
            {
                switch (due)
                {
                    case "Today":
                        string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                        workTasks = workTasks.Where(x => x.DueDate == todayDate && x.Status != "Completed");
                        break;
                    case "Tomorrow":
                        string tomorrowDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                        workTasks = workTasks.Where(x => x.DueDate == tomorrowDate && x.Status != "Completed");
                        break;
                    case "Late":
                        workTasks = workTasks.Where(x => DateTime.Parse(x.DueDate) < DateTime.Now.Date && x.Status == "In progress");
                        break;
                    case "Future":
                        workTasks = workTasks.Where(x => DateTime.Parse(x.DueDate) > DateTime.Now.Date && x.Status != "Completed");
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(priority))
            {
                workTasks = workTasks.Where(x => x.Priority == priority);
            }

            if (!string.IsNullOrEmpty(progress))
            {
                workTasks = workTasks.Where(x => x.Status == progress);
            }
            return workTasks;
        }
    }
}

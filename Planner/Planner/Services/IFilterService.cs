using Planner.Model;

namespace Planner.Services
{
    public interface IFilterService
    {
        IEnumerable<WorkTaskOutput> FilterWorkTask(IEnumerable<WorkTaskOutput> workTasks, string? due, string? priority, string? progress);
    }
}

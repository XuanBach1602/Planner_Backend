using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Repository
{
    public class TemporaryWorkTaskRepository : ITemporaryWorkTaskRepository
    {
        private readonly PlannerDbContext _context;
        public TemporaryWorkTaskRepository(PlannerDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TemporaryWorkTask temporaryWorkTask)
        {
            await _context.TemporaryWorkTasks.AddAsync(temporaryWorkTask);
        }

        public async Task<TemporaryWorkTask> FindById(int id)
        {
            var temporaryWorkTask = await _context.TemporaryWorkTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (temporaryWorkTask == null) { return null; }
            else return temporaryWorkTask;
        }

        public async Task Remove(int id)
        {
            var temporaryWorkTask = await FindById(id);
            if (temporaryWorkTask != null)
            {
                _context.TemporaryWorkTasks.Remove(temporaryWorkTask);
            }
            else throw new Exception("Cannot find the temporary worktask");
        }
    }
}

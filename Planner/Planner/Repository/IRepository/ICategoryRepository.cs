using Planner.Model;
using System.Linq.Expressions;

namespace Planner.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category Category);
        Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? filter = null, string? includeProperties = null);
        Task<Category> GetFirstOrDefaultAsync(Expression<Func<Category, bool>> filter, string? includeProperties = null);
        void Update(Category Category);
        void Remove(Category Category);
    }
}

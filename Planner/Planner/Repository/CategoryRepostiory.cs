using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Linq.Expressions;

namespace Categoryner.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PlannerDbContext _context;
        public CategoryRepository(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category Category)
        {
            await _context.Categories.AddAsync(Category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Category> query = _context.Categories;
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

        public async Task<Category> GetFirstOrDefaultAsync(Expression<Func<Category, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Category> query = _context.Categories;
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

        public void Remove(Category Category)
        {
            _context.Categories.Remove(Category);
        }

        public void Update(Category Category)
        {
            _context.Categories.Update(Category);
        }
    }
}

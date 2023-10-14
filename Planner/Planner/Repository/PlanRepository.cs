﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Planner.Model;
using Planner.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Planner.Repository
    
{
    public class PlanRepository : IPlanRepository
    {
        private readonly PlannerDbContext _context;

        public PlanRepository(PlannerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Plan plan)
        {
            await _context.Plans.AddAsync(plan);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(Expression<Func<Plan, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Plan> query = _context.Plans;
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

        public async Task<Plan> GetFirstOrDefaultAsync(Expression<Func<Plan, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Plan> query = _context.Plans;
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

        public void Remove(Plan plan)
        {
            _context.Plans.Remove(plan);
        }

        public void Update(Plan plan)
        {
            _context.Plans.Update(plan);
        }
    }
}

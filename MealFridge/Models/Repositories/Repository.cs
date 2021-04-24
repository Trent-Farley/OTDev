using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MealFridge.Models.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext ctx)
        {
            _context = ctx;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddOrUpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            if (_dbSet.Any(e => e.Equals(entity)))
                await UpdateAsync(entity);
            _context.Add(entity);
            await _context.SaveChangesAsync(); //Breaking here
            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
                return;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("Entity to delete was null");
            }
            else
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public virtual async Task DeleteByIdAsync(int id)
        {
            await DeleteAsync(await FindByIdAsync(id));
            return;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await FindByIdAsync(id) != null;
        }

        public virtual async Task<TEntity> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }
    }
}
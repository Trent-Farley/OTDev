using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> FindByIdAsync(int id);
        Task<bool> ExistsAsync(int id);

        IQueryable<TEntity> GetAll();

        Task<TEntity> AddOrUpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
        Task DeleteByIdAsync(int id);

    }
}

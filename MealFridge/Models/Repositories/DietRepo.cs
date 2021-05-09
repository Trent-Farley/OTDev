using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Repositories
{
    public class DietRepo : Repository<Diet>, IDietRepo
    {
        public DietRepo(MealFridgeDbContext ctx) : base(ctx) { }

        public Task<Diet> FindByIdAsync(string userId)
        {
            return Task.FromResult(_dbSet.Where(d => d.AccountId == userId).FirstOrDefault());
        }
    }
}

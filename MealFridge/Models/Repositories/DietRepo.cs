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

        public Diet Diet(IQueryable<Diet> diets ,string userId)
        {
            return diets.Where(d => d.AccountId == userId).FirstOrDefault();
        }
    }
}

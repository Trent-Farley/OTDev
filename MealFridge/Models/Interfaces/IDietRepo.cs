using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IDietRepo : IRepository<Diet>
    {
        Diet Diet(IQueryable<Diet> diets, string userId);
    }
}

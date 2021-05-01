using MealFridge.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IRecipeRepo : IRepository<Recipe>
    {
        public List<Recipe> GetRandomSix();
    }
}
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

<<<<<<< HEAD
        public Task SaveDetails(Recipe recipe);
=======
        public Task SaveListOfRecipes(List<Recipe> recipes);
>>>>>>> 9fb5dfd6960d1ac1ba9665637b470a895d1ef24b
    }
}
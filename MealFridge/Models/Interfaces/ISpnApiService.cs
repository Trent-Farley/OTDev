using MealFridge.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface ISpnApiService
    {
        public List<Recipe> SearchApi(Query query);

        public List<Ingredient> SearchIngredients(Query query);
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IIngredientRepo : IRepository<Ingredient>
    {
        List<Ingredient> SearchName(string queryValue);
        Task AddIngredAsync(Ingredient ingredient);
    }
}

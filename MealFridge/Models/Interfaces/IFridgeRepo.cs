using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IFridgeRepo : IRepository<Fridge>
    {
        Task<bool> ExistsAsync(string userId, int ingredId);        
        Task<Fridge> FindByIdAsync(string userId, int ingredId);
        
        List<Fridge> FindByAccount(string userId);
        List<Ingredient> FindByIngredient(int ingredId);
        Task AddFridgeAsync(Fridge fridgeIngredient);
        //IQueryable<Fridge> GetFridgeByAccount(IQueryable<Fridge> fridge, string id);
        //IQueryable<Fridge> GetFridgeById(IQueryable<Fridge> fridge, int id);
    }
}

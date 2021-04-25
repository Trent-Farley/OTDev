using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Repositories
{
    public class FridgeRepo: Repository<Fridge>, IFridgeRepo
    {
        public FridgeRepo(MealFridgeDbContext ctx) : base(ctx) { }

        public async Task<bool> ExistsAsync(string userId, int ingredId)
        {
            return await FindByIdAsync(userId, ingredId) != null;
        }

        public async Task<Fridge> FindByIdAsync(string UserId, int ingredId)
        {
            var r = await _dbSet.FindAsync(UserId, ingredId);
            return r;
        }

        public List<Fridge> FindByAccount(string userId)
        {
            var r = _dbSet.Where(f => f.AccountId == userId).ToList();
            return r;
        }

        public List<Ingredient> FindByIngredient(int ingredId)
        {
            var ingreds = new List<Ingredient>();
            foreach(var i in _dbSet.Where(i => i.IngredId == ingredId))
            {
                ingreds.Add(i.Ingred);
            }
            return ingreds;
        }

        public async Task AddFridgeAsync(Fridge fridgeIngredient)
        {
            if (fridgeIngredient == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            //If you need more, add to shopping list
            if (fridgeIngredient.NeededAmount > fridgeIngredient.Quantity)
                fridgeIngredient.Shopping = true;
            else
                fridgeIngredient.Shopping = false;
            //If you have none, and don't need any, remove the item.
            if (fridgeIngredient.Quantity <= 0 && fridgeIngredient.NeededAmount <= 0)
                await DeleteAsync(fridgeIngredient);
            else if (_dbSet.Any(i => i.AccountId == fridgeIngredient.AccountId && i.IngredId == fridgeIngredient.IngredId))
                _dbSet.Update(fridgeIngredient);
            else
                _dbSet.Add(fridgeIngredient);
            await _context.SaveChangesAsync();
        }
    }
}

using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealFridge.Utils; 

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

            if (fridgeIngredient.Quantity < 0)
                fridgeIngredient.Quantity = 0;
            //If you have none, and don't need any, remove the item.
            if (fridgeIngredient.Quantity <= 0 && fridgeIngredient.NeededAmount <= 0)
                await DeleteAsync(fridgeIngredient);
            else if (_dbSet.Any(i => i.AccountId == fridgeIngredient.AccountId && i.IngredId == fridgeIngredient.IngredId))
                _dbSet.Update(fridgeIngredient);
            else
                _dbSet.Add(fridgeIngredient);
            await _context.SaveChangesAsync();
        }
        //Fix plurals and TryParse Skipping
        public async Task AddRecipeIngred(string userId, Recipeingred r)
        {
            if (await ExistsAsync(userId, r.IngredId))
            {
                var item = await FindByIdAsync(userId, r.IngredId);
                var tempAmount = UnitConverter.Convert(r.Amount ?? 0.0, r.ServingUnit, item.UnitType);
                if (double.IsNaN(tempAmount))
                {
                    //prompt user for input.
                }
                var rounded = UnitConverter.RoundedAmount((double)item.NeededAmount + tempAmount, item.UnitType);
                if(item.Quantity > 0)
                {
                    item.Quantity = UnitConverter.Convert((double)item.Quantity, item.UnitType, rounded.Key); 
                }
                item.NeededAmount = rounded.Value;
                item.UnitType = rounded.Key;
                if (item.NeededAmount > item.Quantity)
                    item.Shopping = true;
            }
            else
            {
                var item = new Models.Fridge
                {
                    AccountId = userId,
                    IngredId = r.IngredId,
                    Quantity = 0,
                    NeededAmount = r.Amount,
                    UnitType = r.ServingUnit,
                    Shopping = true
                };
                //broken
                bool check = false;
                var test = _context.Set<Diet>().Where(d => d.AccountId == userId).FirstOrDefault();
                if (test.Metric != null)
                    if (test.Metric.Value)
                        check = true;
                if (check && UnitConverter.isUs(r.ServingUnit))
                {
                    if (UnitConverter.isMass(r.ServingUnit))
                    {
                        var val = UnitConverter.Convert((double)r.Amount, r.ServingUnit, "gram");
                        var pair = UnitConverter.RoundedAmount(val, "gram");
                        item.NeededAmount = pair.Value;
                        item.UnitType = pair.Key;
                    }
                    else
                    {
                        var val = UnitConverter.Convert((double)r.Amount, r.ServingUnit, "milliliter");
                        var pair = UnitConverter.RoundedAmount(val, "milliliter");
                        item.NeededAmount = pair.Value;
                        item.UnitType = pair.Key;
                    }
                }
                else if (!check && UnitConverter.isMetric(r.ServingUnit))
                {
                    if (UnitConverter.isMass(r.ServingUnit))
                    {
                        var val = UnitConverter.Convert((double)r.Amount, r.ServingUnit, "pound");
                        var pair = UnitConverter.RoundedAmount(val, "pound");
                        item.NeededAmount = pair.Value;
                        item.UnitType = pair.Key;
                    }
                    else
                    {
                        var val = UnitConverter.Convert((double)r.Amount, r.ServingUnit, "cup");
                        var pair = UnitConverter.RoundedAmount(val, "cup");
                        item.NeededAmount = pair.Value;
                        item.UnitType = pair.Key;
                    }
                }
                await AddFridgeAsync(item);
            }
        }
    }
}

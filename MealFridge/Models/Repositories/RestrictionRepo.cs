using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Repositories
{
    public class RestrictionRepo: Repository<Restriction>, IRestrictionRepo
    {
        public RestrictionRepo(MealFridgeDbContext ctx) : base(ctx) { }

        public async Task<List<Ingredient>> RemoveRestrictions(List<Ingredient> ingredients)
        {
            var t = new List<Ingredient>();
            foreach(var i in ingredients)
            {
                if (await ExistsAsync(i.Id)) { }
                else
                    t.Add(i);
            }
            return t;
        }

        public List<Restriction> GetUserRestrictedIngred(IQueryable<Restriction> restrictedIngreds, string userId)
        {
            return restrictedIngreds.Where(u => u.AccountId == userId && u.Banned == true).ToList();
        }

        public virtual List<Restriction> GetUserDislikedIngred(IQueryable<Restriction> restrictedIngreds, string userId)
        {
            return restrictedIngreds.Where(u => u.AccountId == userId && u.Dislike == true).ToList();
        }
        public Restriction Restriction(IQueryable<Restriction> restrictedIngreds, string userId, int ingredId)
        {
            return restrictedIngreds.Where(r => r.AccountId == userId && r.IngredId == ingredId).FirstOrDefault();
        }
    }
}

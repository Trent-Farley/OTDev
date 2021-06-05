using TastyMeals.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TastyMeals.Models.Repositories
{
    public class RestrictionRepo : Repository<Restriction>, IRestrictionRepo
    {
        public RestrictionRepo(TastyMealsDbContext ctx) : base(ctx)
        {
        }

        public async Task<List<Ingredient>> RemoveRestrictions(List<Ingredient> ingredients)
        {
            var t = new List<Ingredient>();
            foreach (var i in ingredients)
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

        //I (Christian) added these functions to include ingredients to this repo for my story
        public List<Restriction> GetUserRestrictedIngredWithIngredName(IQueryable<Restriction> restrictedIngreds, string userId)
        {
            return restrictedIngreds.Where(u => u.AccountId == userId && u.Banned == true).Include(i => i.Ingred).ToList();
        }

        public virtual List<Restriction> GetUserDislikedIngredWithIngredName(IQueryable<Restriction> restrictedIngreds, string userId)
        {
            return restrictedIngreds.Where(u => u.AccountId == userId && u.Dislike == true).Include(i => i.Ingred).ToList();
        }
    }
}
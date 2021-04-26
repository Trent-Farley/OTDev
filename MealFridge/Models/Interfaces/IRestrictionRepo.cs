using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IRestrictionRepo: IRepository<Restriction>
    {
        Task<List<Ingredient>> RemoveRestrictions(List<Ingredient> ingredients);
        Restriction Restriction(IQueryable<Restriction> restrictedIngreds, string userId, int ingredId);
        List<Restriction> GetUserRestrictedIngred(IQueryable<Restriction> restrictedIngreds, string userId);
        List<Restriction> GetUserDislikedIngred(IQueryable<Restriction> restrictedIngreds, string userId);
        List<Restriction> GetUserRestrictedIngredWithIngredName(IQueryable<Restriction> restrictedIngreds, string userId);
        List<Restriction> GetUserDislikedIngredWithIngredName(IQueryable<Restriction> restrictedIngreds, string userId);
    }
}

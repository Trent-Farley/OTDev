using System;
using System.Collections.Generic;
using System.Text;
using TastyMeals.Models;
using TastyMeals.Utils;
using NUnit.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TastyMeals.Tests.Models
{
    class TestSavedrecipes
    {
        private static SavedRecipe MakeSavedRecipe()
        {
            return new SavedRecipe()
            {
                AccountId = "77",
                RecipeId = 121,
                Shelved = false,
                Favorited = true,
                Recipe = new Recipe()
                {
                    Title = "Spinich Wrap"
                }
            };
        }

        [Test]
        public void TestToSeeIfCreatedSavedRecipeIsValid()
        {
            var temp = MakeSavedRecipe();
            Assert.IsNotNull(temp);
        }

        [Test]
        public void TestToCheckForTheSavedrecipeAccountId()
        {
            var temp = MakeSavedRecipe();
            Assert.IsTrue(temp.AccountId == "77");
            temp.AccountId = "87";
            Assert.IsTrue(temp.AccountId != "77" && temp.AccountId == "87");
        }
        
        [Test]
        public void TestToCheckForTheSavedrecipeRecipeId()
        {
            var temp = MakeSavedRecipe();
            Assert.IsTrue(temp.RecipeId == 121);
            temp.RecipeId = 242;
            Assert.IsTrue(temp.RecipeId != 121 && temp.RecipeId == 242);

        }

        [Test]
        public void TestToCheckForTheSavedrecipeShelvedStatus()
        {
            var temp = MakeSavedRecipe();
            Assert.IsFalse(temp.Shelved);
            temp.Shelved = true;
            Assert.IsTrue(temp.Shelved);
        }

        [Test]
        public void TestToCheckForTheSavedrecipeFavoritedStatus()
        {
            var temp = MakeSavedRecipe();
            Assert.IsTrue(temp.Favorited);
            temp.Favorited = false;
            Assert.IsFalse(temp.Favorited);
        }

        [Test]
        public void TestToCheckForTheSavedrecipeRecipeTitle()
        {
            var temp = MakeSavedRecipe();
            Assert.IsTrue(temp.Recipe.Title == "Spinich Wrap");
        }
    }
}

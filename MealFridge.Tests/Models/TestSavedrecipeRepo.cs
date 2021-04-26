using System;
using System.Collections.Generic;
using System.Text;
using MealFridge.Models;
using MealFridge.Utils;
using MealFridge.Models.Interfaces;
using NUnit.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MealFridge.Models.Repositories;

namespace MealFridge.Tests.Models
{
    class TestSavedrecipeRepo
    {
        public Mock<DbSet<Savedrecipe>> mockSavedRecipeDbSet;
        public Mock<MealFridgeDbContext> context = new Mock<MealFridgeDbContext>();
        public ISavedrecipeRepo savedRecipeRepo;
        public List<Savedrecipe> list;

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            return mockSet;
        }
        public List<Savedrecipe> SavedRecipeFactory()
        {
            return new List<Savedrecipe>
            {
                new Savedrecipe { AccountId= "this", RecipeId=7, Favorited = true, Shelved = false, Recipe = null},
                new Savedrecipe { AccountId = "is", RecipeId = 13, Favorited = true, Shelved = false, Recipe = null },
                new Savedrecipe { AccountId = "my", RecipeId = 77, Favorited = false, Shelved = true, Recipe = null },
                new Savedrecipe { AccountId = "test", RecipeId = 666, Favorited = false, Shelved = true, Recipe = null },
                new Savedrecipe { AccountId = "test", RecipeId = 123, Favorited = true, Shelved = false, Recipe = null}
         };
        }

        public void SetupMockEnvironment()
        {
            list = SavedRecipeFactory();
            mockSavedRecipeDbSet = GetMockDbSet(list.AsQueryable());
            context.Setup(ctx => ctx.Savedrecipes).Returns(mockSavedRecipeDbSet.Object);
            savedRecipeRepo = new SavedrecipeRepo(context.Object);
        }

        [Test]
        public void SavedRecipesShouldGetTheShelvedRecipes()
        {
            SetupMockEnvironment();
            var temp = savedRecipeRepo.GetShelvedRecipe("my", list.AsQueryable());
            Assert.IsTrue(temp.Count <= 1);
            foreach (var i in temp)
            {
                Assert.IsTrue(i.AccountId == "my");
                Assert.IsTrue(i.Shelved == true);
                Assert.IsTrue(i.RecipeId == 77);
            }
        }

        [Test]
        public void SavedRecipesShouldNOTGetTheShelvedRecipesIfUserIdIsNull()
        {
            SetupMockEnvironment();
            var temp = savedRecipeRepo.GetShelvedRecipe("fake", list.AsQueryable());
            Assert.IsEmpty(temp);
        }

        [Test]
        public void SavedRecipesShouldGetTheFavoritedRecipes()
        {
            SetupMockEnvironment();
            var temp = savedRecipeRepo.GetFavoritedRecipeWithIQueryable("is", list.AsQueryable());
            Assert.IsTrue(temp.Count == 1);
            foreach (var i in temp)
            {
                Assert.IsTrue(i.AccountId == "is");
                Assert.IsTrue(i.Favorited == true);
                Assert.IsTrue(i.RecipeId == 13);
            }
        }
        [Test]
        public void SavedRecipesShouldNOTGetTheFavoritedRecipesIfUserIdIsNull()
        {
            SetupMockEnvironment();
            var temp = savedRecipeRepo.GetFavoritedRecipeWithIQueryable("not_real", list.AsQueryable());
            Assert.IsEmpty(temp);
        }

        [Test]
        public void SavedRecipesShouldGetAllOfTheRecipes()
        {
            SetupMockEnvironment();
            var temp = savedRecipeRepo.GetAllRecipes("test", list.AsQueryable());
            Assert.IsTrue(temp.Count <= 2);
            foreach (var i in temp)
            {
                Assert.IsTrue(i.AccountId == "test");
                Assert.IsTrue(i.RecipeId == 666 || i.RecipeId == 123);
            }
        }

        [Test]
        public void SavedRecipesShouldNOTGetAllOfTheRecipesIfUserIdIsNull()
        {
            SetupMockEnvironment();
            var temp = savedRecipeRepo.GetAllRecipes("not_right", list.AsQueryable());
            Assert.IsEmpty(temp);
        }
    }
}


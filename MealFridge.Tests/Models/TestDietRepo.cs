//Tests created by Chris Edwards covering User Story 177688839

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
    internal class TestDietRepo
    {
        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            return mockSet;
        }

        [Test]
        public void Diet_LookupDietForUserWhoDoesNotExist()
        {
            List<Diet> diets = new List<Diet>
            {
                new Diet {AccountId = "a", DairyFree=true, GlutenFree= true, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegen=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "b", DairyFree=true, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=true, Vegen=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "c", DairyFree=false, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegen=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "d", DairyFree=false, GlutenFree= false, Keto=false, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegen=false, Vegetarian=false,Whole30=false }
            };

            Mock<DbSet<Diet>> mockDietDbSet = GetMockDbSet(diets.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Diets).Returns(mockDietDbSet.Object);
            IDietRepo dietRepo = new DietRepo(mockContext.Object);

            Assert.That(dietRepo.FindByIdAsync("Chris"), Is.Not.Null);
        }

        [Test]
        public void Diet_LookupDietForUserWhoDoesExist()
        {
            List<Diet> diets = new List<Diet>
            {
                new Diet {AccountId = "a", DairyFree=true, GlutenFree= true, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegen=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "b", DairyFree=true, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=true, Vegen=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "c", DairyFree=false, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegen=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "d", DairyFree=false, GlutenFree= false, Keto=false, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegen=false, Vegetarian=false,Whole30=false }
            };

            Mock<DbSet<Diet>> mockDietDbSet = GetMockDbSet(diets.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Diets).Returns(mockDietDbSet.Object);
            IDietRepo dietRepo = new DietRepo(mockContext.Object);

            //Assert.That( await dietRepo.FindByIdAsync("a").AccountId == "a");     TODO: FIX these tests
            //Assert.That(dietRepo.Diet(diets.AsQueryable(), "b").AccountId == "b" && dietRepo.Diet(diets.AsQueryable(), "b").DairyFree == true);
        }
    }
}
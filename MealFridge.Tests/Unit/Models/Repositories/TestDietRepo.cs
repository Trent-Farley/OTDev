//Tests created by Chris Edwards covering User Story 177688839

using System;
using System.Collections.Generic;
using System.Text;
using TastyMeals.Models;
using TastyMeals.Utils;
using TastyMeals.Models.Interfaces;
using NUnit.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TastyMeals.Models.Repositories;
using System.Threading.Tasks;
using TastyMeals.Tests.Utils;

namespace TastyMeals.Tests.Models
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
        public async Task Diet_LookupDietForUserWhoDoesNotExist()
        {
            List<Diet> diets = new List<Diet>
            {
                new Diet {AccountId = "a", DairyFree=true, GlutenFree= true, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegan=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "b", DairyFree=true, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=true, Vegan=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "c", DairyFree=false, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegan=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "d", DairyFree=false, GlutenFree= false, Keto=false, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegan=false, Vegetarian=false,Whole30=false }
            };

            Mock<DbSet<Diet>> mockDietDbSet = MockObjects.GetMockDbSet<Diet>(diets.AsQueryable());
            mockDietDbSet.Setup(d => d.FindAsync(It.IsAny<string>())).ReturnsAsync((string x) =>
            {
                string id = (string)x;
                return diets.Where(f => (f.AccountId == id)).FirstOrDefault();
            });
            Mock<TastyMealsDbContext> mockContext = new Mock<TastyMealsDbContext>();
            mockContext.Setup(ctx => ctx.Diets).Returns(mockDietDbSet.Object);
            mockContext.Setup(ctx => ctx.Set<Diet>()).Returns(mockDietDbSet.Object);
            IDietRepo dietRepo = new DietRepo(mockContext.Object);

            var chrisAccount = await dietRepo.FindByIdAsync("Chris");

            Assert.That(chrisAccount, Is.Null);
        }

        [Test]
        public async Task Diet_LookupDietForUserWhoDoesExist()
        {
            List<Diet> diets = new List<Diet>
            {
                new Diet {AccountId = "a", DairyFree=true, GlutenFree= true, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegan=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "b", DairyFree=true, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=true, Vegan=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "c", DairyFree=false, GlutenFree= false, Keto=true, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegan=false, Vegetarian=false,Whole30=true },
                new Diet {AccountId = "d", DairyFree=false, GlutenFree= false, Keto=false, LactoVeg=false, OvoVeg=false, Paleo=false, Pescetarian=false, Primal=false, Vegan=false, Vegetarian=false,Whole30=false }
            };

            Mock<DbSet<Diet>> mockDietDbSet = MockObjects.GetMockDbSet<Diet>(diets.AsQueryable());
            mockDietDbSet.Setup(d => d.FindAsync(It.IsAny<string>())).ReturnsAsync((string x) =>
            {
                string id = (string)x;
                return diets.Where(f => (f.AccountId == id)).FirstOrDefault();
            });
            Mock<TastyMealsDbContext> mockContext = new Mock<TastyMealsDbContext>();
            mockContext.Setup(ctx => ctx.Diets).Returns(mockDietDbSet.Object);
            mockContext.Setup(ctx => ctx.Set<Diet>()).Returns(mockDietDbSet.Object);
            IDietRepo dietRepo = new DietRepo(mockContext.Object);

            var a = await dietRepo.FindByIdAsync("a");
            var b = await dietRepo.FindByIdAsync("b");

            Assert.That(a.AccountId == "a" && a.GlutenFree == true);
            Assert.That(b.AccountId == "b" && b.DairyFree == true);
        }
    }
}
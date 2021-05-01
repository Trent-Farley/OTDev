//Test created by Chris Edwards covering User Stories 177688832 and 177688819

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
    class TestRestrictionRepo
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
        public void RestrictionRepo_ReturnUsersDislikedIngredients()
        {
            List<Restriction> restrictions = new List<Restriction>
            {
                new Restriction {AccountId = "a", IngredId = 0, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "b", IngredId = 1, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "c", IngredId = 2, Banned=false, Dislike=true, Ingred = null },
                new Restriction {AccountId = "d", IngredId = 3, Banned=false, Dislike=true, Ingred = null }
            };
            Mock<DbSet<Restriction>> mockRestrictionDbSet = GetMockDbSet(restrictions.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Restrictions).Returns(mockRestrictionDbSet.Object);
            IRestrictionRepo restrictionRepo = new RestrictionRepo(mockContext.Object);

            var dislikedIngreds = restrictionRepo.GetUserDislikedIngred(restrictions.AsQueryable(), "c");

            Assert.That(dislikedIngreds.Count == 1);
            Assert.That(dislikedIngreds[0].IngredId == 2);
        }
        [Test]
        public void RestrictionRepo_DislikedIngredientsforUserWhoDoesNotExistShouldReturnEmptyList()
        {
            List<Restriction> restrictions = new List<Restriction>
            {
                new Restriction {AccountId = "a", IngredId = 0, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "b", IngredId = 1, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "c", IngredId = 2, Banned=false, Dislike=true, Ingred = null },
                new Restriction {AccountId = "d", IngredId = 3, Banned=false, Dislike=true, Ingred = null }
            };
            Mock<DbSet<Restriction>> mockRestrictionDbSet = GetMockDbSet(restrictions.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Restrictions).Returns(mockRestrictionDbSet.Object);
            IRestrictionRepo restrictionRepo = new RestrictionRepo(mockContext.Object);

            var dislikedIngreds = restrictionRepo.GetUserDislikedIngred(restrictions.AsQueryable(), "Chris");

            Assert.That(dislikedIngreds.Count == 0);
            Assert.That(dislikedIngreds.Count < 1);
        }
        [Test]
        public void RestrictionRepo_ReturnUsersRestrictedIngredients()
        {
            List<Restriction> restrictions = new List<Restriction>
            {
                new Restriction {AccountId = "a", IngredId = 0, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "b", IngredId = 1, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "c", IngredId = 2, Banned=false, Dislike=true, Ingred = null },
                new Restriction {AccountId = "d", IngredId = 3, Banned=false, Dislike=true, Ingred = null }
            };
            Mock<DbSet<Restriction>> mockRestrictionDbSet = GetMockDbSet(restrictions.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Restrictions).Returns(mockRestrictionDbSet.Object);
            IRestrictionRepo restrictionRepo = new RestrictionRepo(mockContext.Object);

            var restrictedIngreds = restrictionRepo.GetUserRestrictedIngred(restrictions.AsQueryable(), "a");

            Assert.That(restrictedIngreds.Count == 1);
            Assert.That(restrictedIngreds[0].IngredId == 0);
            Assert.That(restrictedIngreds[0].AccountId != "b");
            Assert.That(restrictedIngreds.Count < 2);
        }
        [Test]
        public void RestrictionRepo_RestrictedIngredientsForUserWhoDoesNotExistShouldReturnEmptyList()
        {
            List<Restriction> restrictions = new List<Restriction>
            {
                new Restriction {AccountId = "a", IngredId = 0, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "b", IngredId = 1, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "c", IngredId = 2, Banned=false, Dislike=true, Ingred = null },
                new Restriction {AccountId = "d", IngredId = 3, Banned=false, Dislike=true, Ingred = null }
            };
            Mock<DbSet<Restriction>> mockRestrictionDbSet = GetMockDbSet(restrictions.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Restrictions).Returns(mockRestrictionDbSet.Object);
            IRestrictionRepo restrictionRepo = new RestrictionRepo(mockContext.Object);

            var restrictedIngreds = restrictionRepo.GetUserRestrictedIngred(restrictions.AsQueryable(), "Chris");

            Assert.That(restrictedIngreds.Count == 0);
            Assert.That(restrictedIngreds.Count < 1);
        }
        [Test]
        public void RestrictionRepo_RestrictionShouldReturnASingleRestriction()
        {
            List<Restriction> restrictions = new List<Restriction>
            {
                new Restriction {AccountId = "a", IngredId = 0, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "b", IngredId = 1, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "c", IngredId = 2, Banned=false, Dislike=true, Ingred = null },
                new Restriction {AccountId = "d", IngredId = 3, Banned=false, Dislike=true, Ingred = null }
            };
            Mock<DbSet<Restriction>> mockRestrictionDbSet = GetMockDbSet(restrictions.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Restrictions).Returns(mockRestrictionDbSet.Object);
            IRestrictionRepo restrictionRepo = new RestrictionRepo(mockContext.Object);

            var restriction1 = restrictionRepo.Restriction(restrictions.AsQueryable(), "a", 0);
            var restriction2 = restrictionRepo.Restriction(restrictions.AsQueryable(), "b", 1);
            var restriction3 = restrictionRepo.Restriction(restrictions.AsQueryable(), "c", 2);
            var restriction4 = restrictionRepo.Restriction(restrictions.AsQueryable(), "d", 3);

            Assert.That(restriction1.AccountId == "a" && restriction1.Banned == true);
            Assert.That(restriction2.AccountId == "b" && restriction2.Banned == true);
            Assert.That(restriction3.AccountId == "c" && restriction3.Banned == false);
            Assert.That(restriction4.AccountId == "d" && restriction4.Banned == false);
        }
        [Test]
        public void RestrictionRepo_Restriction_LookupRestrictionthatDoesNotExist()
        {
            List<Restriction> restrictions = new List<Restriction>
            {
                new Restriction {AccountId = "a", IngredId = 0, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "b", IngredId = 1, Banned=true, Dislike=false, Ingred = null },
                new Restriction {AccountId = "c", IngredId = 2, Banned=false, Dislike=true, Ingred = null },
                new Restriction {AccountId = "d", IngredId = 3, Banned=false, Dislike=true, Ingred = null }
            };
            Mock<DbSet<Restriction>> mockRestrictionDbSet = GetMockDbSet(restrictions.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Restrictions).Returns(mockRestrictionDbSet.Object);
            IRestrictionRepo restrictionRepo = new RestrictionRepo(mockContext.Object);

            var restriction = restrictionRepo.Restriction(restrictions.AsQueryable(), "Chris", 4);

            Assert.That(restriction == null);
        }
    }
}


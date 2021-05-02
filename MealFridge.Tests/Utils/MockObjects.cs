using Castle.Core.Logging;
using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace MealFridge.Tests.Utils
{
    public static class MockObjects
    {
        public static Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(() => entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(() => entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(() => entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => entities.GetEnumerator());
            return mockSet;
        }

        private static Recipe[] CreateRecipes(int count)
        {
            var recipes = new Recipe[count];
            var titles = Enumerable.Range(0, count)
                .Select(i => "Recipe{" + i + "}")
                .ToList();
            for (var i = 0; i < count; ++i)
            {
                recipes[i] = new Recipe
                {
                    Title = titles[i],
                    Id = i,
                    Dinner = true,
                    Lunch = true,
                    Breakfast = true
                };
            }
            return recipes;
        }

        public static Mock<IRecipeRepo> CreateMockRecipeRepo(int count)
        {
            var mockRepo = new Mock<IRecipeRepo>();
            mockRepo.Setup(g => g.GetAll())
                .Returns(CreateRecipes(count).AsQueryable());
            mockRepo.Setup(r => r.GetRandomSix())
                .Returns(CreateRecipes(count).AsQueryable().OrderBy(i => i.Id).Take(6).ToList());
            mockRepo.Setup(a => a.AddOrUpdateAsync(It.IsAny<Recipe>()));
            mockRepo.Setup(a => a.SaveListOfRecipes(It.IsAny<List<Recipe>>()));
            return mockRepo;
        }

        public static Mock<UserManager<IdentityUser>> CreateUserMock()
        {
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            mockStore.Setup(x => x.FindByIdAsync("1", CancellationToken.None))
                .ReturnsAsync(new IdentityUser()
                {
                    UserName = "test@email.com",
                    Id = "1"
                });
            var mockUserManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(
                new IdentityUser
                {
                    Id = "1",
                    Email = "test@email.com"
                });
            return mockUserManager;
        }

        public static Mock<HttpContext> CreateMockContext()
        {
            var mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(ctx => ctx.User.Identity.Name).Returns("test@email.com");
            return mockContext;
        }

        public static Mock<ISpnApiService> CreateApiService()
        {
            var mockService = new Mock<ISpnApiService>();
            mockService.Setup(t => t.SearchApi(It.IsAny<Query>())).Returns(CreateRecipes(10).ToList());
            /* TODO Mock getting ingredients here */
            return mockService;
        }

        public static IConfiguration CreateConfig()
        {
            var memSettings = new Dictionary<string, string>
            {
                { "SApiKey", "Test" }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(memSettings)
                .Build();
            return config;
        }

        public static Savedrecipe[] CreateSavedRecipes(int count)
        {
            var svRecipes = new Savedrecipe[count];
            for (var i = 0; i < count; ++i)
            {
                svRecipes[i] = new Savedrecipe
                {
                    AccountId = "1",
                    Recipe = new Recipe { Id = i },
                    Favorited = true
                };
            }
            return svRecipes;
        }

        public static Mock<ISavedrecipeRepo> CreateSavedRecipeMock(int count)
        {
            var saved = new Mock<ISavedrecipeRepo>();
            saved.Setup(s => s.GetFavoritedRecipe(It.IsAny<string>())).Returns(CreateSavedRecipes(count).ToList());
            return saved;
        }
    }
}
using Castle.Core.Logging;
using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    }
}
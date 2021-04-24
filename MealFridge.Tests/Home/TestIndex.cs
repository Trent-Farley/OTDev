using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealFridge.Tests.Utils;
using MealFridge.Controllers;
using Microsoft.AspNetCore.Mvc;
using MealFridge.Models;

namespace MealFridge.Tests.Home
{
    /// <summary>
    /// Tester: Trent Farley
    /// User Story ID: #177689339
    /// Background: I ended up having to test the HomeController even though it didn't relate to my story
    /// because I needed it to seed the db on the first run for my story. Since I *broke it/fixed it* I decided
    /// I should probably test it too.
    /// </summary>
    [TestFixture]
    public class TestIndex
    {
        [Test]
        public async Task TestIndexWithEnoughRecipes_Should_ReturnPopulatedEnumerable()
        {
            //Arrange
            var recipeRepo = MockObjects.CreateMockRecipeRepo(10);
            var spnService = MockObjects.CreateApiService();
            var controller = new HomeController(MockObjects.CreateConfig(), recipeRepo.Object, spnService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = MockObjects.CreateMockContext().Object
                }
            };
            //Act
            var results = await controller.Index();
            var data = (results as ViewResult).ViewData.Model as IEnumerable<Recipe>;
            //Assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Count(), Is.EqualTo(6));
            Assert.That(data.ElementAt(0).Id, Is.EqualTo(0));
        }

        [Test]
        public async Task TestIndexWithZero_Should_ReturnEmptyEnumerable()
        {
            //Arrange
            var recipeRepo = MockObjects.CreateMockRecipeRepo(0);
            var spnService = MockObjects.CreateApiService();

            var controller = new HomeController(MockObjects.CreateConfig(), recipeRepo.Object, spnService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = MockObjects.CreateMockContext().Object
                }
            };
            //Act
            var results = await controller.Index();
            var data = (results as ViewResult).ViewData.Model as IEnumerable<Recipe>;
            //Assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task TestIndexWithZero_Should_InvokeGetSixFunction()
        {
            //Arrange
            var recipeRepo = MockObjects.CreateMockRecipeRepo(0);
            var spnService = MockObjects.CreateApiService();

            var controller = new HomeController(MockObjects.CreateConfig(), recipeRepo.Object, spnService.Object);
            //Act
            await controller.Index();
            //Assert
            Assert.That(recipeRepo.Invocations, Is.Not.Null);
            Assert.That(recipeRepo.Invocations.Count(), Is.Not.EqualTo(0));
        }

        [Test]
        public async Task TestIndexWithZero_Should_InvokeSpnFunction()
        {
            //Arrange
            var recipeRepo = MockObjects.CreateMockRecipeRepo(0);
            var spnService = MockObjects.CreateApiService();
            var controller = new HomeController(MockObjects.CreateConfig(), recipeRepo.Object, spnService.Object);
            //Act
            await controller.Index();
            //Assert
            Assert.That(spnService.Invocations, Is.Not.Null);
            Assert.That(spnService.Invocations.Count(), Is.EqualTo(3));
        }
    }
}
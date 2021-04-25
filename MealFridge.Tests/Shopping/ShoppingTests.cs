using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Models.Repositories;
using MealFridge.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MealFridge.Tests.Shopping
{
    class ShoppingTests
    {

        private Mock<DbSet<Fridge>> _mockFridgeDbSet;
        private Mock<MealFridgeDbContext> _mockContext;
        private List<Fridge> _data;
        [SetUp]
        public void Setup()
        {
            _data = new List<Fridge>
            {
                new Fridge
                {
                    AccountId = "1",
                    IngredId = 1,
                    Quantity = 1,
                    NeededAmount = 0,
                    Shopping = false
                },
                new Fridge
                {
                    AccountId = "1",
                    IngredId = 2,
                    Quantity = 0,
                    NeededAmount = 1,
                    Shopping = true
                },
                new Fridge
                {
                    AccountId = "1",
                    IngredId = 3,
                    Quantity = 0,
                    NeededAmount = 1,
                    Shopping = true
                }
            };

            _mockFridgeDbSet = MockObjects.GetMockDbSet<Fridge>(_data.AsQueryable());
            _mockFridgeDbSet.Setup(d => d.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] x) =>
            {
                string id = (string)x[0];
                int ingredId = (int)x[1];
                return _data.Where(f => (f.AccountId == id) && (f.IngredId == ingredId)).FirstOrDefault();
            });
            _mockFridgeDbSet.Setup(d => d.Add(It.IsAny<Fridge>()))
                .Callback<Fridge>(d => _data.Add(d));
            _mockFridgeDbSet.Setup(d => d.Update(It.IsAny<Fridge>()))
                .Callback<Fridge>(d =>
                { 
                    _data.Remove(_data.Where(f => f.AccountId == d.AccountId && f.IngredId == d.IngredId).FirstOrDefault());
                    _data.Add(d);
                });

            _mockContext = new Mock<MealFridgeDbContext>();
            _mockContext.Setup(ctx => ctx.Fridges).Returns(_mockFridgeDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Fridge>()).Returns(_mockFridgeDbSet.Object);
            _mockContext.Setup(d => d.Fridges.Remove(It.IsAny<Fridge>()))
                .Callback<Fridge>((f) => _data.Remove(f));
            //_mockContext.Setup(d => d.Fridges.AddAsync(It.IsAny<Fridge>(), default).Result)
            //                            .Returns((EntityEntry<Fridge>)null);
        }

        [Test]
        public async Task ShoppingList_RemovingValidIngredientSucceeds() 
        {
            //Arrange
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = await fridgeRepo.FindByIdAsync("1", 2);
            //Act
            await fridgeRepo.DeleteAsync(item);
            //Assert
            Assert.IsFalse(fridgeRepo.GetAll().Contains(item));
            Assert.AreEqual(fridgeRepo.GetAll().Count(), 2);
        }
        [Test]
        public async Task ShoppingList_RemovingInvalidIngredientFails()
        {
            //Arrange
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = new Fridge
            {
                AccountId = "1",
                IngredId = 5,
                NeededAmount = 1,
                Quantity = 0,
                Shopping = true
            };
            //Act
            await fridgeRepo.DeleteAsync(item);
            //Assert
            Assert.IsFalse(fridgeRepo.GetAll().Contains(item));
            Assert.AreEqual(fridgeRepo.GetAll().Count(), 3);
        }
        [Test]
        public async Task ShoppingList_AddingValidIngredientSucceeds()
        {
            //Arrange
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = new Fridge
            {
                AccountId = "1",
                IngredId = 5,
                NeededAmount = 1,
                Quantity = 0,
                Shopping = true
            };
            //Act
            await fridgeRepo.AddFridgeAsync(item);
            //Assert
            Assert.IsTrue(fridgeRepo.GetAll().Contains(item));
            Assert.AreEqual(fridgeRepo.GetAll().Count(), 4);
        }
        [Test]
        public async Task ShoppingList_AddingInvalidIngredientFails()
        {
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = new Fridge
            {
                AccountId = "1",
                IngredId = 5,
                NeededAmount = 0,
                Quantity = 0,
                Shopping = true
            };
            //Act
            await fridgeRepo.AddFridgeAsync(item);
            //Assert
            Assert.IsFalse(fridgeRepo.GetAll().Contains(item));
        }
        [Test]
        public async Task ShoppingList_SettingIngredientAmountSucceeds()
        {
            //Arrange
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = await fridgeRepo.FindByIdAsync("1", 2);
            //Act
            item.NeededAmount += 1;
            await fridgeRepo.AddFridgeAsync(item);
            //Assert
            Assert.IsTrue(fridgeRepo.GetAll().Contains(item));
            Assert.AreEqual(await fridgeRepo.FindByIdAsync("1", 2), item);
        }
        [Test]
        public async Task ShoppingList_SettingIngredientAmountEqualOrBelowZeroRemoves()
        {
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = await fridgeRepo.FindByIdAsync("1", 2);
            //Act
            item.NeededAmount = 0;
            await fridgeRepo.AddFridgeAsync(item);
            //Assert
            Assert.IsFalse(fridgeRepo.GetAll().Contains(item));
        }
        [Test]
        public async Task ShoppingList_AddingRecipeWithStoredIngredients()
        {
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = await fridgeRepo.FindByIdAsync("1", 1);
            //Act
            item.NeededAmount = 2;
            await fridgeRepo.AddFridgeAsync(item);
            //Assert
            Assert.IsTrue(fridgeRepo.GetAll().Contains(item));
            Assert.IsTrue((await fridgeRepo.FindByIdAsync("1", 1)).Shopping);
        }
        [Test]
        public async Task ShoppingList_RemovingIngredientWithStoredInventory()
        {
            IFridgeRepo fridgeRepo = new FridgeRepo(_mockContext.Object);
            var item = await fridgeRepo.FindByIdAsync("1", 1);
            //Act
            item.NeededAmount = 0;
            await fridgeRepo.AddFridgeAsync(item);
            //Assert
            Assert.IsTrue(fridgeRepo.GetAll().Contains(item));
            Assert.IsFalse((await fridgeRepo.FindByIdAsync("1", 1)).Shopping);
        }
    }
}

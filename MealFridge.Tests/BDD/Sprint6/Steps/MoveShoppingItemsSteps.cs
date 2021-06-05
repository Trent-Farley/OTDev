﻿using System;
using System.Collections.Generic;
using System.Globalization;
using TastyMeals.Tests.BDD.Sprint6.Selenium.Driver;
using TastyMeals.Tests.BDD.Sprint6.Selenium.PageObjects;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace TastyMeals.Tests.BDD.Sprint6.Steps
{
    [Binding]
       
    public class MoveShoppingItemsSteps
    {
        private readonly ShoppingPageObject _shoppingPage;
     
        public MoveShoppingItemsSteps(BrowserDriver browserDriver)
        {
            _shoppingPage = new ShoppingPageObject(browserDriver.Current);
        }

        [Given(@"there are items clicked in the list")]
        public void GivenThereAreItemsInTheList()
        {
            if (_shoppingPage.CheckList())
                _shoppingPage.ClickAllCheckboxes();
            else
            {
                _shoppingPage.AddItems();
                _shoppingPage.ClickAllCheckboxes();
            }
            
        }
        [Given(@"there are two items clicked in the list")]
        public void GivenThereAreTwoItemsClickedInTheList()
        {
            if (_shoppingPage.CheckList())
                _shoppingPage.ClickFirstTwoCheckboxes();
            else
            {
                _shoppingPage.AddItems();
                _shoppingPage.ClickFirstTwoCheckboxes();
            }
        }

        [When(@"a user clicks the add button")]
        public void WhenAUserClicksTheAddButton()
        {
            _shoppingPage.ClickAdd();
        }
        
        [When(@"a user clicks the remove button")]
        public void WhenAUserClicksTheRemoveButton()
        {
            _shoppingPage.ClickRemove();
        }
        
        [Then(@"two items are removed from the shopping list")]
        public void ThenTwoItemsAreRemovedFromTheShoppingList_()
        {
            var start = _shoppingPage.ItemAmount;
            var result = _shoppingPage.WaitForResult();
            Assert.IsTrue(start - 2 == result || (start - 2 < 0 && result == 0));
        }

        [Then(@"the items are added to the inventory and removed from the shopping list")]
        public void ThenTheItemsAreAddedToTheInventoryAndRemovedFromTheShoppingList_()
        {
            //Need another PageObject for fridge to see they were added to it.
            var start = _shoppingPage.ItemAmount;
            var result = _shoppingPage.WaitForResult();
            Assert.IsTrue(start - 2 == result || (start - 2 < 0 && result == 0));
        }
        
        [Then(@"all items are removed from the shopping list")]
        public void ThenAllItemsAreRemovedFromTheShoppingList_()
        {
            var actualResult = _shoppingPage.WaitForResult();
            Assert.AreEqual(0, actualResult);
        }
    }
}

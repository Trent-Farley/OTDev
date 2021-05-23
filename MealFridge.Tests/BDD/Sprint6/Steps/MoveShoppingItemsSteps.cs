using System;
using System.Collections.Generic;
using MealFridge.Tests.BDD.Sprint6.Selenium.Driver;
using MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace MealFridge.Tests.BDD.Sprint6.Steps
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
        
        [Then(@"the items are added to the inventory and removed from the shopping list\.")]
        public void ThenTheItemsAreAddedToTheInventoryAndRemovedFromTheShoppingList_()
        {
            var actualResult = _shoppingPage.WaitForResult();
            Assert.AreEqual(0, actualResult);
        }
        
        [Then(@"all items are removed from the shopping list\.")]
        public void ThenAllItemsAreRemovedFromTheShoppingList_()
        {
            var actualResult = _shoppingPage.WaitForResult();
            Assert.AreEqual(0, actualResult);
        }
    }
}

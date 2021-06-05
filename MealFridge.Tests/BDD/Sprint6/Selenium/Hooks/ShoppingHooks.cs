using System;
using System.Collections.Generic;
using System.Text;

using TastyMeals.Tests.BDD.Sprint6.Selenium.Driver;
using TastyMeals.Tests.BDD.Sprint6.Selenium.PageObjects;
using TechTalk.SpecFlow;

namespace TastyMeals.Tests.BDD.Sprint6.Selenium.Hooks
{
    [Binding]
    class ShoppingHooks
    {
        [BeforeScenario("ShoppingList")]
        public static void BeforeScenario(BrowserDriver browserDriver)
        {
            var spo = new ShoppingPageObject(browserDriver.Current);
            spo.EnsureShoppingListIsOpen();
        }
    }
}

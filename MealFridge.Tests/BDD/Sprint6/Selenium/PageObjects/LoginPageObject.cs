using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects
{
    class LoginPageObject
    {
        private const string LoginUrl = "https://localhost:5001/Identity/Account/Login";
        private readonly IWebDriver _webDriver;
        private readonly string user = "test@test.com";
        private readonly string pass = "T35tp@55";
        private IWebElement LoginEmail => _webDriver.FindElement(By.Id("Input_Email"));
        private IWebElement LoginPass => _webDriver.FindElement(By.Id("Input_Password"));
        private IWebElement LoginSubmit => _webDriver.FindElements(By.ClassName("btn"))[0];
        public LoginPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }
        public void EnsureLoginIsOpen()
        {
            if (_webDriver.Url != LoginUrl)
            {
                _webDriver.Navigate().GoToUrl(LoginUrl);
            }
        }

        public void Login()
        {
            EnsureLoginIsOpen();
            LoginEmail.SendKeys(user);
            LoginPass.SendKeys(pass);
            LoginSubmit.Click();
        }
    }
}

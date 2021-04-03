using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealFridge.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MealFridge.Models;
namespace MealFridge.Tests.Home
{
    [TestFixture]
    public class TestIndex
    {

        public ILogger CreateLogger()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            return factory.CreateLogger<HomeController>();
        }

        [Test]
        public void TestMethod()
        {

        }
    }
}

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

namespace MealFridge.Tests.Models
{
    class TestRestrictions
    {
        private static Restriction MakeRestriction()
        {
            return new Restriction()
            {
                AccountId = "101",
                Banned = true,
                Dislike = false,
                IngredId = 77,
                Ingred = new Ingredient()
                {
                    Name = "Peanuts",
                }
            };
        }

        [Test]
        public void TestToSeeIfCreatedGetRestrictionIsNotNull()
        {
            var temp = MakeRestriction();
            Assert.IsNotNull(temp);
        }

        [Test]
        public void TestForGetRestrictionAccountID()
        {
            var temp = MakeRestriction();
            Assert.IsTrue(temp.AccountId == "101");
            temp.AccountId = "42";
            Assert.IsTrue(temp.AccountId != "101" && temp.AccountId == "42");
        }

        [Test]
        public void TestForGetRestrictionBannedStatus()
        {
            var temp = MakeRestriction();
            Assert.IsTrue(temp.Banned == true);
            temp.Banned = false;
            Assert.IsTrue(temp.Banned == false);
        }
       
        [Test]
        public void TestToSeeIfGetRestrictionDislikeStatusIsFalse()
        {
            var temp = MakeRestriction();
            Assert.IsTrue(temp.Dislike == false);
        }

        [Test]
        public void TestToSeeIfGetRestrictionDislikeStatusIsTrue()
        {
            var temp = MakeRestriction();
            temp.Dislike = true;
            Assert.IsTrue(temp.Dislike == true);
        }


        [Test]
        public void TestToSeeIfGetRestrictionIngredientName()
        {
            var temp = MakeRestriction();
            Assert.IsTrue(temp.Ingred.Name == "Peanuts");
        }

        [Test]
        public void TestToSeeIfGetRestrictionIngredientInformationSuchAsCost()
        {
            var temp = MakeRestriction();
            Assert.IsNull(temp.Ingred.Cost);
            temp.Ingred.Cost = 5;
            Assert.IsTrue(temp.Ingred.Cost == 5);
        }

        [Test]
        public void TestForGetRestrictionIngredientInformationSuchAsCalories()
        {
            var temp = MakeRestriction();
            Assert.IsNull(temp.Ingred.Calories);
            temp.Ingred.Calories = 50;
            Assert.IsTrue(temp.Ingred.Calories == 50);
        }

        [Test]
        public void TestForGetRestrictionIngredientId()
        {
            var temp = MakeRestriction();
            Assert.IsTrue(temp.IngredId == 77);
            temp.IngredId = 78;
            Assert.IsTrue(temp.IngredId != 77 && temp.IngredId == 78);
        }

    }
}

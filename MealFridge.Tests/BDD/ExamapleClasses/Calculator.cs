using System;
using System.Collections.Generic;
using System.Text;

namespace TastyMeals.Tests.Business.Features
{
    public class Calculator
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }

        public int Add()
        {
            return FirstNumber + SecondNumber;
        }
    }
}
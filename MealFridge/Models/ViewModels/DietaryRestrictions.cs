﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.ViewModels
{
    public class DietaryRestrictions
    {
        public List<Restriction> userRestrictions { get; set; }
        public Diet Diet { get; set; }
    }
}

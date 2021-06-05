﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Interfaces
{
    public interface IDietRepo : IRepository<Diet>
    {
        Task<Diet> FindByIdAsync(string userId);
    }
}

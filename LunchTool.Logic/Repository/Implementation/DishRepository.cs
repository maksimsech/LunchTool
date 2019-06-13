﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Enities;
using LunchTool.Logic.Context;

namespace LunchTool.Logic.Repository.Implementation
{
    public class DishRepository : IRepository<Dish>
    {
        private DataContext dataContext;

        public DishRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<Dish> GetAll() => dataContext.Dishes;

        public Dish Get(int id) => dataContext.Dishes.Find(id);

        public void Add(Dish item) => dataContext.Dishes.Add(item);

        public void Update(Dish item) => dataContext.Entry(item).State = EntityState.Modified;

        public IEnumerable<Dish> Find(Func<Dish, bool> predicate) => dataContext.Dishes.Where(predicate).ToList();

        public void Delete(int id)
        {
            var dish = dataContext.Dishes.Find(id);
            if (dish != null)
                dataContext.Dishes.Remove(dish);
        }
    }
}

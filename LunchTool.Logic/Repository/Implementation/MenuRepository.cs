using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Enities;
using LunchTool.Logic.Context;

namespace LunchTool.Logic.Repository.Implementation
{
    public class MenuRepository : IRepository<Menu>
    {
        private DataContext dataContext;

        public MenuRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<Menu> GetAll() => dataContext.Menus;

        public Menu Get(int id) => dataContext.Menus.Find(id);

        public void Add(Menu item) => dataContext.Menus.Add(item);

        public void Update(Menu item) => dataContext.Entry(item).State = EntityState.Modified;

        public IEnumerable<Menu> Find(Func<Menu, bool> predicate) => dataContext.Menus.Where(predicate).ToList();

        public void Delete(int id)
        {
            var menu = dataContext.Menus.Find(id);
            if (menu != null)
                dataContext.Menus.Remove(menu);
        }
    }
}

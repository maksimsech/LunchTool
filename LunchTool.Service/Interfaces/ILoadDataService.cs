using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LunchTool.Service.DTO;

namespace LunchTool.Service.Interfaces
{
    public interface ILoadDataService
    {
        IEnumerable<ProviderDTO> GetAllProviders();
        IEnumerable<ProviderDTO> GetProviders(Expression<Func<ProviderDTO, bool>> predicate);

        IEnumerable<MenuDTO> GetAllMenus();
        IEnumerable<MenuDTO> GetMenus(Expression<Func<MenuDTO, bool>> predicate);

        IEnumerable<DishDTO> GetAllDishes();
        IEnumerable<DishDTO> GetDishes(Expression<Func<DishDTO, bool>> predicate);
    }
}

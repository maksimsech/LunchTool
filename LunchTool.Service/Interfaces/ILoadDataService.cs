using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Service.DTO;

namespace LunchTool.Service.Interfaces
{
    public interface ILoadDataService
    {
        IEnumerable<ProviderDTO> GetAllProviders();
        IEnumerable<ProviderDTO> GetProviders(Func<ProviderDTO, bool> predicate);

        IEnumerable<MenuDTO> GetAllMenus();
        IEnumerable<MenuDTO> GetMenus(Func<MenuDTO, bool> predicate);

        IEnumerable<DishDTO> GetAllDishes();
        IEnumerable<DishDTO> GetDishes(Func<DishDTO, bool> predicate);
    }
}

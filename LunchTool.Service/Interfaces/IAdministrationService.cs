using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Service.DTO;

namespace LunchTool.Service.Interfaces
{
    public interface IAdministrationService
    {
        void AddProvider(ProviderDTO providerDTO);
        void RemoveProvider(ProviderDTO providerDTO);
        ICollection<ProviderDTO> GetProviders(Func<ProviderDTO, bool> predicate);

        void AddMenu(MenuDTO menuDTO);
        void RemoveMenu(MenuDTO menuDTO);
        ICollection<MenuDTO> GetMenus(Func<MenuDTO, bool> predicate);

        void AddDish(DishDTO dishDTO);
        void RemoveDish(DishDTO dishDTO);
        ICollection<DishDTO> GetDishes(Func<DishDTO, bool> predicate);
    }
}

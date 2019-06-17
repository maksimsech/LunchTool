using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Interfaces.AdministrationServices
{
    public interface IDishService
    {
        void Add(DishDTO dishDTO);
        void Change(DishDTO dishDTO);
        void Delete(DishDTO dishDTO);
    }
}

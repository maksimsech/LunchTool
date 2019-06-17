using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Interfaces.AdministrationServices
{
    public interface IMenuService
    {
        void Add(MenuDTO menuDTO);
        void Change(MenuDTO menuDTO);
        void Delete(MenuDTO menuDTO);
    }
}

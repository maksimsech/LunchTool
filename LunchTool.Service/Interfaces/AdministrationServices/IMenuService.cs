using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Interfaces.AdministrationServices
{
    public interface IMenuService
    {
        int Add(MenuDTO menuDTO);
        void Change(MenuDTO menuDTO);
        void Delete(MenuDTO menuDTO);
        void Activate(MenuDTO menuDTO);
        void Deactivate(MenuDTO menuDTO);
    }
}

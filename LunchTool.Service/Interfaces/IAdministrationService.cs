using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces.AdministrationServices;

namespace LunchTool.Service.Interfaces
{
    public interface IAdministrationService
    {
        IDishService Dish { get; }
        IMenuService Menu { get; }
        IProviderService Provider { get; }
    }
}

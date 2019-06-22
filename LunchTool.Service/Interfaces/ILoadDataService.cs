using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces.LoadDataServices;

namespace LunchTool.Service.Interfaces
{
    public interface ILoadDataService
    {
        IDishService Dishes { get; }
        IMenuService Menus { get; }
        IProviderService Providers { get; }
        IUserService Users { get; }
    }
}

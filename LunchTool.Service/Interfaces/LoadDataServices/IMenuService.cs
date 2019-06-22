using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LunchTool.Service.Interfaces.LoadDataServices
{
    public interface IMenuService
    {
        IEnumerable<MenuDTO> GetAll();
        IEnumerable<MenuDTO> Get(Expression<Func<MenuDTO, bool>> predicate);
    }
}

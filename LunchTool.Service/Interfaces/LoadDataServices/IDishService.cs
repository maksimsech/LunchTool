using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LunchTool.Service.Interfaces.LoadDataServices
{
    public interface IDishService
    {
        IEnumerable<DishDTO> GetAll();
        IEnumerable<DishDTO> Get(Expression<Func<DishDTO, bool>> predicate);
    }
}

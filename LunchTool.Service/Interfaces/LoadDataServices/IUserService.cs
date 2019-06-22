using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LunchTool.Service.Interfaces.LoadDataServices
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAll();
        IEnumerable<UserDTO> Get(Expression<Func<UserDTO, bool>> predicate);
    }
}

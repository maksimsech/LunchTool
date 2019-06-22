using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LunchTool.Service.Interfaces.LoadDataServices
{
    public interface IProviderService
    {
        IEnumerable<ProviderDTO> GetAll();
        IEnumerable<ProviderDTO> Get(Expression<Func<ProviderDTO, bool>> predicate);
    }
}

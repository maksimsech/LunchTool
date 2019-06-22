using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces;
using LunchTool.Service.Interfaces.LoadDataServices;

namespace LunchTool.Service.Implementation
{
    public partial class LoadDataService
    {
        public class ProviderService: IProviderService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public ProviderService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProviderDTO, Provider>();
                    cfg.CreateMap<Provider, ProviderDTO>();
                }).CreateMapper();
            }

            public IEnumerable<ProviderDTO> GetAll()
            {
                var providers = db.Providers.GetAll();
                var providersDTO = mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderDTO>>(providers);
                return providersDTO;
            }

            public IEnumerable<ProviderDTO> Get(Expression<Func<ProviderDTO, bool>> predicate)
            {
                var fun = mapper.MapExpression<Expression<Func<Provider, bool>>>(predicate).Compile();
                var providers = db.Providers.Find(fun);
                var providersDTO = mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderDTO>>(providers);
                return providersDTO;
            }
        }
    }
}

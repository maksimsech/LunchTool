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
        public class MenuService: IMenuService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public MenuService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MenuDTO, Menu>();
                    cfg.CreateMap<Menu, MenuDTO>();
                }).CreateMapper();
            }

            public IEnumerable<MenuDTO> GetAll()
            {
                var menus = db.Menus.GetAll();
                var menusDTO = mapper.Map<IEnumerable<Menu>, IEnumerable<MenuDTO>>(menus);
                return menusDTO;
            }

            public IEnumerable<MenuDTO> Get(Expression<Func<MenuDTO, bool>> predicate)
            {
                var fun = mapper.MapExpression<Expression<Func<Menu, bool>>>(predicate).Compile();
                var menus = db.Menus.Find(fun);
                var menusDTO = mapper.Map<IEnumerable<Menu>, IEnumerable<MenuDTO>>(menus);
                return menusDTO;
            }
        }
    }
}
